from flask import Flask, request
from flask_restful import Api
import mysql.connector
import requests
import logging
import paramiko
import json

app = Flask(__name__)

api = Api(app)

def check_name(full_name, first_last_name):
    first_name, last_name = first_last_name.split()
    return (first_name in full_name) and (last_name in full_name)

@app.route('/autorizacao', methods=['GET', 'POST'])
def verifica_autorizacao():
    if request.method == 'GET':
        pass
    elif request.method == "POST":
        logging.basicConfig(level=logging.INFO)
        args = request.get_json()

        data_nasc = args["data_nascimento"]
        nome_sobrenome = args["nome_sobrenome"]
        chave_cliente = args["chave_cliente"]
        split_nome_sobrenome = nome_sobrenome.upper().split(' ')

        logging.info('Verificação de autorização solicitada, dados: ' + data_nasc + ' - ' + nome_sobrenome + ' - ' + chave_cliente)

        day, month, year = data_nasc.split('/')
        data_nasc = f"{year}-{month}-{day}"

        query = 'SELECT id, nomeCompleto, ddi, celular, auth_whatsapp_em FROM tblCliente WHERE dataNascimento =  \'' + data_nasc + '\' AND chave = \'' + chave_cliente + '\''
        logging.info(query)

        cnxn = mysql.connector.connect(
            host="srvwww.lucassolutions.com.br",
            user="reportmedbr_app",
            password="ypfPXjTN3STEm4rw",
            database="reportmedbr_app"
        )
        cursor = cnxn.cursor(buffered=True)

        logging.info(query)
        cursor.execute(query)

        if cursor.rowcount > 0:
            for i in cursor:
                # verifica se nome bate
                auth = check_name(split_nome_sobrenome, str(i[1].upper()))
                #se bater nome e auth_whatsapp_em nao for nula
                if auth is True:
                    telefonePaciente = str(i[2]) + str(i[3]).replace('(', '').replace(')', '').replace('+',
                                                                                                       '').replace(
                        '-', '')
                    if i[4] != None:
                        logging.info('Paciente autorizado, dados: ' + nome_sobrenome + ' - ' + telefonePaciente + ' - ' + str(i[4]))
                        return '1-' + telefonePaciente, 200
                    else:
                        logging.info('Paciente encontrado, porém não autorizado: ' + telefonePaciente)
                        return'0-' + telefonePaciente, 200

        logging.info('nao autorizado')
        return '0-2', 200


@app.route('/webhook', methods=['GET', 'POST'])
def verifica_mensagem():
    if request.method == 'GET':
        pass
    elif request.method == "POST":
        logging.basicConfig(level=logging.INFO)
        logging.info('entrou')
        args = request.get_json()
        logging.info(args)

        ddi = args["telefone"][0:2]

        tel = args["telefone"][2:-5]
        tel9 = tel[2:4] + '9' + tel[4:]
        logging.info(tel)

        telefone_completo = ddi+tel

        mensagem = args["data_nascimento"]
        #porta = args["porta"] # Porta url whatsapp

        mydb = mysql.connector.connect(
            host="srvwww.lucassolutions.com.br",
            user="reportmedbr_app",
            password="ypfPXjTN3STEm4rw",
            database="reportmedbr_app"
        )
        cursor = mydb.cursor()
        if 'não'.upper() in mensagem.upper():
            query = 'SELECT id FROM tblCliente WHERE ddi = \'' + ddi + '\' AND celular = \'' + tel + '\' OR ddi = \'' + ddi + '\' AND celular = \'' + tel9 + '\''
            cursor.execute(query)

            if cursor.rowcount == 0:
                # mensagem registro nao encontrado
                json_mensagem = {
                    "number": telefone_completo,
                    "message": "Pedimos desculpas pelo inconveniente. Não entraremos mais em contato através do Whatsapp. Desejamos a você um ótimo dia!"
                }

                logging.info(json_mensagem)

                requests.post('http://rmwhatsapp/zdg-message',
                              data=json_mensagem,
                              headers={"content-type": "application/x-www-form-urlencoded"})
            else:
                for i in cursor:
                    none = (None)
                    query = "UPDATE tblCliente SET auth_whatsapp_em = %s WHERE id = \'" + i[
                        0] + "\'"
                    cursor.execute(query, none)
                    mydb.commit()

                json_mensagem = {
                    "number": telefone_completo,
                    "message": "Entendido, confirmamos que a autorização de envio de exames foi removida com sucesso da nossa base de dados. Agradecemos o seu contato e desejamos um ótimo dia."
                }

                logging.info(json_mensagem)

                requests.post('http://rmwhatsapp/zdg-message',
                              data=json_mensagem,
                              headers={"content-type": "application/x-www-form-urlencoded"})

        else:
            query = 'SELECT id, statusVerificacao FROM tblCliente WHERE ddi = \'' + ddi + '\' AND celular = \'' + tel + '\' OR ddi = \'' + ddi + '\' AND celular = \'' + tel9 + '\''
            logging.info(query)

            cursor.execute(query)

            if cursor.rowcount > 0:
                for r in cursor:
                    if str(r[1]) == '0':

                        json_mensagem = {
                            "number": telefone_completo,
                            "message": "Para prosseguir, seria necessário confirmar o seu primeiro nome. Poderia fornecê-lo, por favor?"
                        }

                        logging.info(json_mensagem)

                        requests.post('https://api-ws.report-med.com/zdg-message',
                                      data=json_mensagem,
                                      headers={"content-type": "application/x-www-form-urlencoded"})

                        query = 'UPDATE tblCliente SET statusVerificacao = 1 WHERE id = \'' + str(r[0]) + '\''
                        cursor.execute(query)
                        mydb.commit()

                    if str(r[1] == '1'):
                        # query nome e telefone paciente, se bater continua, se nao avisa que nao bateu
                        query_nome_paciente = 'SELECT id, statusVerificacao FROM tblCliente WHERE ddi = \'' + ddi + '\' AND celular = \'' + tel + '\' AND nomeCompleto LIKE  \'%' + mensagem + '%\' OR ddi = \'' + ddi + '\' AND celular = \'' + tel9 + '\' AND nomeCompleto LIKE  \'%' + mensagem + '%\''
                        logging.info(query_nome_paciente)
                        cursor.execute(query_nome_paciente)

                        if cursor.rowcount == 0:
                            json_mensagem = {
                                "number": telefone_completo,
                                "message": "Pedimos desculpas, mas não encontramos registros correspondentes a esse nome e número de telefone em nosso banco de dados. Se você digitou o nome incorretamente, por favor, envie novamente para que possamos refazer a verificação. Caso o problema persista, recomendamos que entre em contato diretamente com nosso parceiro para resolver essa questão."
                            }

                            logging.info(json_mensagem)

                            requests.post('http://rmwhatsapp/zdg-message',
                                          data=json_mensagem,
                                          headers={"content-type": "application/x-www-form-urlencoded"})
                        else:
                            json_mensagem = {
                                "number": telefone_completo,
                                "message": "Agradecemos, " + mensagem + ". Agora, precisamos da sua data de nascimento, por favor. Por favor, responda apenas com a data no formato dd/mm/aaaa, como por exemplo, 16/12/1998."
                            }

                            logging.info(json_mensagem)

                            requests.post('https://api-ws.report-med.com/zdg-message',
                                          data=json_mensagem,
                                          headers={"content-type": "application/x-www-form-urlencoded"})

                            query = 'UPDATE tblCliente SET statusVerificacao = 2 WHERE id = \'' + str(r[0]) + '\''
                            cursor.execute(query)
                            mydb.commit()
                    elif str(r[1]) == '2':
                        # query nome e telefone paciente e data nascimento, se bater continua, se nao avisa que nao bateu
                        try:
                            data = '-'.join(mensagem.split('/')[::-1])
                        except:
                            data = ''

                        query_data_paciente = 'SELECT id, statusVerificacao FROM tblCliente WHERE ddi = \'' + ddi + '\' AND celular = \'' + tel + '\' AND dataNascimento =  \'' + data + '\' OR ddi = \'' + ddi + '\' AND celular = \'' + tel9 + '\' AND dataNascimento =  \'' + data + '\''
                        logging.info(query_data_paciente)

                        cursor.execute(query_data_paciente)

                        if cursor.rowcount == 0:
                            json_mensagem = {
                                "number": telefone_completo,
                                "message": "Pedimos desculpas, mas não encontramos registros correspondentes a essa data de nascimento e número de telefone em nosso banco de dados. Se você digitou o nome incorretamente, por favor, envie novamente para que possamos refazer a verificação. Caso o problema persista, recomendamos que entre em contato diretamente com nosso parceiro para resolver essa questão."
                            }

                            logging.info(json_mensagem)

                            requests.post('http://rmwhatsapp/zdg-message',
                                          data=json_mensagem,
                                          headers={"content-type": "application/x-www-form-urlencoded"})
                        else:
                            json_mensagem = {
                                "number": telefone_completo,
                                "message": "Dados confirmados. A Clínica XYZ agradece e estará enviando seus exames para este número de WhatsApp."
                            }

                            logging.info(json_mensagem)

                            requests.post('https://api-ws.report-med.com/zdg-message',
                                          data=json_mensagem,
                                          headers={"content-type": "application/x-www-form-urlencoded"})

                            query = 'UPDATE tblCliente SET statusVerificacao = 3, auth_whatsapp_em = NOW() WHERE id = \'' + str(r[0]) + '\''
                            cursor.execute(query)
                            mydb.commit()

                            logging.info('Vai verificar se tem exame pendente')
                            #verificar se paciente tem exame pendente a ser enviado na tabela bilhetagem
                            query_exame_pendente_envio = 'SELECT parceiro.porta, exames.chave, paciente.numeroRegistro, bilhetagem.StudyInstanceUid, exames.dataExame FROM tblBilhetagemPartner as bilhetagem left join tblExameCliente as exames on exames.id = bilhetagem.idAgendamento left join tblCliente  as paciente on paciente.id = exames.cliente left join tblPartners as parceiro on parceiro.idParceiro = bilhetagem.idParceiro  WHERE status = 0 AND paciente.id = \'' + str(r[0]) + '\''
                            logging.info(query_exame_pendente_envio)
                            cursor.execute(query_exame_pendente_envio)

                            if cursor.rowcount > 0:
                                for i in cursor:
                                    logging.info('Tem exame pendente')
                                    logging.info(i)

                                    #montar url do caminho do exame
                                    porta_sd = i[0]#porta servidor dicom
                                    chave_cliente = i[1]
                                    numero_registro_paciente = i[2]
                                    study_id = i[3]
                                    data_exame = i[4]

                                    if porta_sd and chave_cliente and numero_registro_paciente and study_id and data_exame:
                                        ano_exame = data_exame[0:4]
                                        mes_exame = data_exame[5:7]
                                        dia_exame = data_exame[8:10]
                                    else:
                                        logging.info('Erro ao montar url exame')
                                        logging.info(str(porta_sd) + ' - ' + str(chave_cliente) + ' - ' + str(numero_registro_paciente) + ' - ' + str(study_id) + ' - ' + str(data_exame))
                                        continue

                                    url_base = 'https://media.report-med.com/'
                                    server = "159.69.171.8"
                                    username = "root"
                                    password = "Ls001008**++"
                                    path = '/opt/report-med/store/' + str(porta_sd) + '/' + chave_cliente + '/' + numero_registro_paciente +\
                                           '/' + study_id + '/' + ano_exame + '/' + mes_exame + '/' + dia_exame

                                    url_sem_nome_arquivo = url_base + path.replace('/opt/report-med/store/', '')
                                    logging.info(url_sem_nome_arquivo)

                                    #caminhar até a pasta em que se encontra as imagens e  listar as imagens
                                    file_names = None
                                    try:
                                        transport = paramiko.Transport((server, 22))
                                        transport.connect(username=username, password=password)
                                        sftp = paramiko.SFTPClient.from_transport(transport)
                                        file_names = sftp.listdir(path)
                                        sftp.close()
                                    except Exception as e:
                                        logging.info(f"An error occurred: {str(e)}")

                                    if file_names is not None:
                                        for file_name in file_names:
                                            # terminar de montar urls com o nome dos arquivos
                                            urlArquivo = url_sem_nome_arquivo + '/' + file_name
                                            logging.info(urlArquivo)
                                            # enviar
                                            json_data = {
                                                "number": telefone_completo,
                                                "file": urlArquivo,
                                                "caption": "Exame",
                                            }
                                            response = requests.post(
                                                'https://api-ws.report-med.com/zdg-media',
                                                headers={
                                                    "content-type": "application/x-www-form-urlencoded"
                                                },
                                                data=json_data,
                                            )
                                            logging.info(response.text)

                                            res_json = json.loads(response.text)

                                            if 'BOT-LS Imagem enviada' in res_json["message"]:
                                                logging.info('mensagem enviada')
                                            else:
                                                logging.info('mensagem nao enviada')
                                    else:
                                        logging.info("Failed to retrieve file list.")
                    break
        return '200'

if __name__ == '__main__':
    host = '0.0.0.0'
    logging.info('host: ' + host)
    app.run(host, 8003, True)  # run our Flask app
   # app.run('192.168.10.18')  # run our Flask app
