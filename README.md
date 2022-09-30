# NModbusNext
A fresh take on the NModbus project

# Terminology

https://modbus.org/

## Client-Server

Per the Modbus org: https://www.modbus.org/docs/Client-ServerPR-07-2020-final.docx.pdf

|Term|Definition|
|---|---|
|Client|Initiates communication and makes requests of server device(s).|
|Server|Processes requests and return an appropriate response (or error message).|
|Coils|Coils are 1-bit registers, are used to control discrete outputs, and may be read or written.|
|DiscreteInputs|1-bit registers used as inputs, and may only be read.|
|HoldingRegisters|16-bit registers used for input, and may only be read.|
|InputRegisters|16-bit registers, may be read or written, and may be used for a variety of things including inputs, outputs, configuration data, or any requirement for "holding" data.|