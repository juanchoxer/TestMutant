# TestMutant
API - ML

El proceso para detectar si es mutante es el siguiente:

Determino que es un mutante si encuentro por lo menos 2 cadenas de 4 letras repetidas. Estas series las llamaré "matches"

El primer paso es generar la grilla con los datos de dna informados.
Para determinar la cantidad de columnas, tomo la longitud de los strings recibidos, y para determinar la cantidad de filas tomo la cantidad de strings recibidos.

Voy a utilizar dos punteros para recorrer la grilla:
	-uno para indicar las casillas que voy recorriendo (puntero inicial)
	-un puntero comparador para analizar las 4 posibles direcciones (este, suroeste, sur y sureste).


**Funciones**

- ValidateDNAAndBuildGrid: Se encarga de poblar la grilla con la info del dna. Adicionalmente valido que las letras sean válidos. Este paso se realiza conjuntamente para evitar múltiples recorridos del dna.

- FindMatches: es el método principal que recorre toda la grilla y busca todas las posibles coincidencias. Si encuentro por lo menos 2, se deja de buscar.

- CheckMatches: se busca a partir del puntero inicial, analizando las 4 posibles direcciones con el puntero comparador

- DirectionIsAMatch: se busca una dirección específica utilizando el puntero comparador para validar si encuentra 4 coincidencias seguidas, convirtiéndose en un match. Si encuentra que la letra no es la misma que la letra incial, deja de buscar en esa dirección

- MovePointerInDirection: Para recorrer las 4 posibles direcciones, utilizo 2 arrays auxiliares (dirRow y dirCol) que indican cómo debería ser el movimiento del puntero para cada dirección
Ej: para la dirección este, se agrega 0 a las filas y 1 a las columnas. Para el suroeste, se suma 1 a las filas y se resta 1 a las columnas.

- ReadLetter: determino al char ' ' como una casilla inexistente. Si las coordenadas están dentro de la grilla, devuelvo la letra correspondiente.

- MovePointer: mueve el puntero principal para recorrer toda la grilla.



** Posibles respuestas **

200: El dna pertenece a un mutante
403: El dna pertenece a un no mutante (humano)
409: El dna informado contiene caracteres inválidos


++Posibles accesos ++

[GET]
.../api/stats -> Accede a la base de datos para obtenre la información de los dna ya procesados.

[POST]
../api/mutant -> Permite enviar en el body un dna (string[]) para saber si es un mutante o no, además de agrarlo a la bd de procesados.

