# MilK Run TP Final

Titulo y genero: Milk Run. Arcade.
Descripción
En Milk Run, el jugador controla a un abuelo que debe cruzar una calle caótica dividida en baldosas cambiantes para llegar a la tienda de enfrente (tienda_final) y comprar leche antes de que cierre. En cada oleada, las baldosas parpadean en color amarillo como advertencia antes de volverse rojas y letales, obligando al personaje a moverse sin pausa hacia las casillas verdes seguras. El jugador gana si logra pisar la plataforma blanca frente a la tienda antes de que el cronómetro de 50 segundos llegue a cero. Pierde la partida si pisa una baldosa roja activa o si se agota el tiempo, terminando arrestado por la policía.
Mecánica principal y ciclo de juego
Desplazamiento ortogonal paso a paso entre casillas leyendo el patrón de colores del suelo y el sonido de advertencia.
Observar el inicio de la oleada (baldosas en amarillo + sonido de cronómetro) → identificar la casilla verde adyacente garantizada → desplazarse antes del parpadeo final a rojo letal → avanzar fila por fila hasta pisar la meta (plataforma) antes de que expire el reloj general (0:50).
Contenido y duración
Solo tiene un modo. Hasta ahora tiene un solo nivel. La partida dura entre 30 segundos y 50 segundos.
Controles actuales 
Flechas.WASD. Movimiento direccional de 4 direcciones (Arriba, Abajo, Izquierda, Derecha.)
Pantalla
Formato 16:9. 1920 × 1080 píxeles. Funciona en pantalla completa. No funciona en ventana. 
Assets y licencias
Creación Propia (Blender y Paint Tool SAI):

Blender 3D: Modelo 3D del personaje principal, texturizado y animaciones; modelo 3D del policía; modelado de los botones de interfaz y de las baldosas del tablero.
Paint Tool SAI: Ilustraciones 2D del abuelo en menús y composición de pantallas.

Modelos 3D de Terceros (Sketchfab):

Ciudad (ciudad): Low Poly City.
Señal vial (cartel): Road Sign.
Cartón de leche: Low Poly Milk Carton.

Tipografía, Iconos y Fondos 2D:

Fuente tipográfica: Cute Dino, convertida a TextMeshPro SDF con delineado (Outline + Dilate) y utilizada en el texto de los botones.
Iconografía UI: Símbolo de retorno, señal "WASD", engranaje de opciones y reloj despertador extraídos de Google Imágenes y editados para el proyecto.
Fondos de pantallas: Los fondos de las pantallas de victoria, arresto y de las dos páginas de EscenaIntro utilizan imágenes extraídas de Google Imágenes combinadas con arte propio.


Música y Efectos de Sonido (SFX):

Música Menú e Intro (8bit Bossa): Bossa Nova de OpenGameArt.
Música Gameplay: Kevin MacLeod - Itty Bitty 8 Bit.
Efectos de Sonido (8 audios): Obtenidos de Pixabay Sound Effects para pasos, baldosa amarilla, baldosa roja, alerta policial, derrota, llegada a tienda, victoria, clic de botón y paso de página.


Estado
• Flujo completo en PC (MenuPrincipal → EscenaIntro → GameplayScene), mecánicas de grilla, cronómetro con modo pánico, sistema de audio en 3 canales y condiciones de victoria y derrota funcionando en Unity 6.3 (VERIFICADO).

• Configuración de Canvas Scaler en 1920x1080 (VERIFICADO).

• Bloqueo estricto en orientación Horizontal (Landscape), adaptación a controles táctiles, SafeArea y expansión de niveles para Android (PROPUESTO).


