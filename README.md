# NModbus.v4

https://modbus.org/

A fresh take on the NModbus project. This approach aims to:

- Make it easier to add / replace custom function implementations on both client and server.
- Rename types to be in line with the Client / Server nomenclature and fundamental types (e.g. `DiscreteInputs`) in the Modbus spec.
- Get rid of the [cruft](https://en.wikipedia.org/wiki/Cruft) from previous versions (old framework support, etc)
- Use an [async](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/task-asynchronous-programming-model) first / only approach.
- Put a more robust / reliable build pipeline in place leveraging [github actions](https://github.com/features/actions).

# Terminology

Client / Server is per the Modbus org: https://www.modbus.org/docs/Client-ServerPR-07-2020-final.docx.pdf

|Term|Definition|
|---|---|
|Client|Initiates communication and makes requests of server device(s).|
|Server|Processes requests and return an appropriate response (or error message).|
|Coils|Coils are 1-bit registers, are used to control discrete outputs, and may be read or written.|
|DiscreteInputs|1-bit registers used as inputs, and may only be read.|
|HoldingRegisters|16-bit registers, may be read or written, and may be used for a variety of things including inputs, outputs, configuration data, or any requirement for "holding" data.|
|InputRegisters|16-bit registers used for input, and may only be read.|


