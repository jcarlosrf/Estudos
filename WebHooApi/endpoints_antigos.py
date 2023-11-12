def chdir(ftp_path, ftp_conn):
    dirs = [d for d in ftp_path.split('/') if d != '']
    print(dirs)
    for p in dirs:
        filelist = check_dir(p, ftp_conn)
    filelist = []
    ftp_conn.retrlines('LIST', filelist.append)
    return filelist


def check_dir(dir, ftp_conn):
    filelist = []
    ftp_conn.retrlines('LIST', filelist.append)
    found = False

    for f in filelist:
        print(dir)
        print(f.split('>          ')[-1])
        if f.split('>          ')[-1] == dir:
            found = True

    if not found:
        try:
            x = ftp_conn.mkd(dir)
            print(x)
        except:
            pass
    print(dir)
    ftp_conn.cwd(dir)
    return filelist


@app.route('/banco', methods=['GET', 'POST'])
def api_insere_banco():
    if request.method == 'GET':
        pass
    elif request.method == "POST":
        print('entrou')

        args = request.get_json()
        tel = args["tel"]
        tel_from = args["number_from"]
        data_nasc = args["data_nasc"]
        pasta_imagens = args["pasta_imagens"]
        try:
            reportzap = args["reportzap"]
        except:
            reportzap = '0'

        print(tel)
        print(data_nasc)
        print(pasta_imagens)
        print(tel_from)
        tel9 = tel[0:4] + '9' + tel[4:]

        data = datetime.datetime.today()
        dia = data.day
        mes = data.month
        ano = data.year

        b = base64.b64encode(bytes(str(((int(dia) + int(mes) + int(ano)) * int(dia))), 'utf-8'))

        # Insere banco
        cnxn = mysql.connector.connect(user='lucassolutions_reportzap', password='ToWxO5HSFRuW7ucz',
                              host='srvwww.lucassolutions.com.br',
                              database='lucassolutions_reportzap')

        cursor = cnxn.cursor(buffered=True)

        query = 'SELECT id FROM envio_exames WHERE status != \'3\' AND data_nascimento =  \'' + data_nasc + '\' AND tel_to = \'' + tel + '\' OR tel_to = \'' + tel9 + '\' '
        print(query)
        cursor.execute(query)

        if cursor.rowcount > 0:
            print('paciente autorizado')
            pass
        else:
            res = requests.get('https://api.lucassolutions.com.br/ApiLsAgent/api/PegarLinks?telefone=' + str(tel_from),
                               headers={'Authorization': b.decode('utf-8')}
                               )
            print(res.text[-5:-1])
            porta = res.text[-5:-1]
            json_mensagem = {
                "number": tel,
                "message": "Nós agradecemos a preferência e ficamos felizes em participar desse momento importante na sua vida.\n\n"
                           "Para enviar seu exame, precisamos confirmar sua data de nascimento, pode nos informar ? Por gentileza informar no formato dd/mm/aaaa. Ex: 16/12/1998."
            }

            print(json_mensagem)

            x = requests.post('http://pacs.report-med.com:' + str(porta) + '/zdg-message', data=json_mensagem,
                              headers={"content-type": "application/x-www-form-urlencoded"})

            query = 'INSERT INTO envio_exames (tel_to, tel_from, mensagem, data_hora, status, token, data_nascimento, exames, reportzap)' \
                    'VALUES (\'' + tel + '\', \''+tel_from+'\', \'\',  \''+str(datetime.datetime.now())[:19]+'\', 1, \'\', \'' + data_nasc + '\', \'' + pasta_imagens.replace('\\', '/' ) + '\', \'' + reportzap + '\')'
            print(query)
            cursor.execute(query)
            cnxn.commit()

            cursor.close()
        return '200'  # return data with 200 OK

@app.route('/cronStatusEnvio', methods=['GET', 'POST'])
def cronStatusEnvio():
    if request.method == 'GET':
        pass
    elif request.method == "POST":
        print('entrou')
        cnxn = mysql.connector.connect(user='lucassolutions_reportzap', password='ToWxO5HSFRuW7ucz',
                                       host='srvwww.lucassolutions.com.br',
                                       database='lucassolutions_reportzap')

        cursor = cnxn.cursor(buffered=True)

        query = 'SELECT exames as PastaExames, tel_to as TelefonePaciente, id, tel_from FROM envio_exames WHERE status = \'2\''

        data = datetime.datetime.today()
        dia = data.day
        mes = data.month
        ano = data.year

        b = base64.b64encode(bytes(str(((int(dia) + int(mes) + int(ano)) * int(dia))), 'utf-8'))

        print(b.decode('utf-8'))
        print(query)
        cursor.execute(query)
        if cursor.rowcount > 0:

            for i in cursor:
                try:
                    time.sleep(2)
                    print(str(i[2]))
                    query_update = 'UPDATE envio_exames SET status = \'3\' WHERE id = \'' + str(i[2]) + '\''
                    print(query_update)
                    cursor.execute(query_update)
                    cnxn.commit()

                    print(i)
                    res = requests.get('https://api.lucassolutions.com.br/ApiLsAgent/api/PegarLinks?telefone=' + str(i[3]),
                                       headers={'Authorization': b.decode('utf-8')}
                                       )
                    print(res.text[-5:-1])


                    host_ftp = 'saas.lucassolutions.com.br'
                    usuario = '6xLwrjPcW6V30EEMhvFN'
                    senha = r'6rpB|?RVZ+g<"o3Si7U,+aJu5LKdw;pC}4M_D/`)\n1;f#o,1A'

                    session = ftplib.FTP()
                    session.connect(host_ftp, 60021)
                    session.login(usuario, senha)
                    #session.prot_p()
                    session.encoding = 'utf-8'

                    listaArquivos = chdir(i[0], session)

                    cont = 1
                    if listaArquivos:
                        for r in listaArquivos:
                            arquivo = r.split(' ')[-1]
                            print(arquivo)

                            url_arquivo = r'https://ws.saas.lucassolutions.com.br/'+i[0][4:]+ '/' + arquivo
                            print(url_arquivo)
                            data = {
                                "number": i[1],
                                "file": url_arquivo,
                                "caption": "Media " + str(cont),
                            }

                            header = {
                                "content-type": "application/x-www-form-urlencoded",
                            }

                            print(data)
                            print('http://pacs.report-med.com:'+res.text[-5:-1]+'/zdg-media')
                            x = requests.post('http://pacs.report-med.com:'+res.text[-5:-1]+'/zdg-media', data=data, headers=header)
                            print(x.url)
                            print(x.text)
                            cont += 1
                except Exception as e:
                    print(e)
                    print('Erro cron')



                    #for r in listaArquivos:
                    #    arquivo = r.split(' ')[-1]
                    #    session.delete(arquivo)
        cursor.close()
        return '200'  # return data with 200 OK

