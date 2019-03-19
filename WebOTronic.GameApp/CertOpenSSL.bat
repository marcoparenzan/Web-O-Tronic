@set RANDFILE=.rnd
openssl genrsa -des3 -passout pass:1234 -out server.key 2048
openssl rsa -passin pass:1234 -in server.key -out server.key
openssl req -sha256 -new -key server.key -out server.csr -subj "/CN=localhost"
openssl x509 -req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt
openssl pkcs12 -export -out cert.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:1234
