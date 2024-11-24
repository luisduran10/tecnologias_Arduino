const int btnContinue = 2; // Botón para avanzar
const int btnZero1 = 3;    // Primer bit '0'
const int btnZero2 = 4;    // Segundo bit '0'
const int btnStop = 5;     // Botón para detener

String bitStream = "";     // Almacena la secuencia de bits generada
String sentence = "";      // Guarda el conjunto de caracteres en una frase
String prevChar = "";      // Último carácter procesado

void setup() {
    pinMode(btnContinue, INPUT_PULLUP);
    pinMode(btnZero1, INPUT_PULLUP);
    pinMode(btnZero2, INPUT_PULLUP);
    pinMode(btnStop, INPUT_PULLUP);

    Serial.begin(9600);
    Serial.println("Listo para recibir datos...");
}

void loop() {
    // Verifica el estado de cada botón
    bool btnStateContinue = digitalRead(btnContinue) == HIGH; // Detecta si está presionado
    bool btnStateZero1 = digitalRead(btnZero1) == HIGH;
    bool btnStateZero2 = digitalRead(btnZero2) == HIGH;
    bool btnStateStop = digitalRead(btnStop) == HIGH;

    // Pausa breve para evitar lecturas erróneas
    delay(3000);

    // Genera la cadena binaria a partir del estado de los botones
    String currentStream = "";
    currentStream += btnStateContinue ? '1' : '0';
    currentStream += btnStateZero1 ? '1' : '0';
    currentStream += btnStateZero2 ? '1' : '0';
    currentStream += btnStateStop ? '1' : '0';

    // Si no hay pulsaciones, reinicia el bucle
    if (currentStream == "0000") {
        return;
    }

    // Añade los bits actuales a la secuencia total
    bitStream += currentStream;

    // Muestra los bits en serie
    for (int i = 0; i < currentStream.length(); i++) {
        Serial.print(currentStream[i]);
        delay(200);
    }
    Serial.println();

    // Cuando se acumulan 8 bits, los convierte en un carácter ASCII
    if (bitStream.length() >= 8) {
        String asciiStream = bitStream.substring(0, 8);
        bitStream = bitStream.substring(8); // Elimina los bits ya procesados

        char asciiCharacter = strtol(asciiStream.c_str(), nullptr, 2); // Conversión binaria a ASCII
        Serial.print("Letra recibida: ");
        Serial.println(asciiCharacter);

        // Actualiza la última letra recibida y la frase
        prevChar = asciiCharacter;
        sentence += asciiCharacter; // Agrega el carácter a la frase

        Serial.print("Frase acumulada: ");
        Serial.println(sentence);
    }

    Serial.println("Esperando más datos...");
    delay(500);
}
