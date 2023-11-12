import requests
from http.client import HTTPSConnection
from base64 import b64encode
from requests.auth import HTTPBasicAuth

#import webbrowser

#webbrowser.open('http://example.com')

#https://filesamples.com/samples/video/mp4/sample_640x360.mp4
#https://upload.wikimedia.org/wikipedia/commons/e/eb/Author_Demetrius_Purvis_II.jpg
#https://filesamples.com/samples/image/jpg/sample_640%C3%97426.jpg
#5527988138794
'''
data = {
    "number": "5527988138794",
    "file": "https://ws.saas.lucassolutions.com.br/reportzap/5521971496699/16-12-1998_DANIEL-AZEVEDO-ROZINDO_4_1.2.276.0.26.1.1.1.2.2023.100.52551.6258018.12451840.jpg",

    "caption": "Teste ReportZap",
}

header = {
    "content-type":"application/x-www-form-urlencoded",
}

print(data)
x = requests.post('http://ws.lucassolutions.com.br:8002/zdg-media', data=data, headers=header)
print(x.text)
'''
data = {
    "number": "5514981257797",
    "message": "teste"
}

print(data)
x = requests.post('http://ws.lucassolutions.com.br:8002/zdg-message', data=data, headers={"content-type":"application/x-www-form-urlencoded"})
print(x.text)

'''

data = {
    "number": "5531999440274",
    "nome": "Lucas Solutions",
    "email": "mmarinho@lucassolutions.com.br",
    "telefone": "5511966631960",
    "info": "",
}

header = {
    "content-type":"application/x-www-form-urlencoded",
}

print(data)
x = requests.post('https://ws.lucassolutions.com.br/send-vcard', data=data, headers=header)
print(x.url)
print(x.text)'''

