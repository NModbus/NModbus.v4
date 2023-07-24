# Create a new key / certificate signing request
openssl req -new -newkey rsa:4096 -nodes -keyout ../certificates/modbus-test.key -out ../certificates/modbus-test.csr

# NOTE: Set CN (common name) to "localhost"
openssl x509 -req -sha256 -days 365 -in ../certificates/modbus-test.csr -signkey ../certificates/modbus-test.key -out ../certificates/modbus-test.pem

# Export the pfx
openssl pkcs12 -export -in ../certificates/modbus-test.pem -inkey ../certificates/modbus-test.key -out ../certificates/modbus-test.pfx