# Milk Run

**Grupo:** MCA  
**Integrantes:** Carolina María de los Angeles Cortez, Valeria Emilia Caballero, Agustin Santiago Ponce  
**Motor y versión:** Unity 6.3 (`6000.3.11f1 LTS`)  
**Repositorio:** [MilK-Run-TP-Final](https://github.com/ChiaroCoure/MilK-Run-TP-Final/tree/main)

## Actividad 1 · Ficha del Juego

| Campo | Qué debe contener |
| :--- | :--- |
| **Título y género** | **Milk Run**. Arcade. |
| **Descripción** | En *Milk Run*, el jugador controla a un abuelo que debe cruzar una calle caótica dividida en baldosas cambiantes para llegar a la tienda de enfrente (`tienda_final`) y comprar leche antes de que cierre. En cada oleada, las baldosas parpadean en color amarillo como advertencia antes de volverse rojas y letales, obligando al personaje a moverse sin pausa hacia las casillas verdes seguras. El jugador gana si logra pisar la plataforma blanca frente a la tienda antes de que el cronómetro de 50 segundos llegue a cero. Pierde la partida si pisa una baldosa roja activa o si se agota el tiempo, terminando arrestado por la policía. |
| **Mecánica principal y ciclo de juego** | • Desplazamiento ortogonal paso a paso entre casillas leyendo el patrón de colores del suelo y el sonido de advertencia.<br>• Observar el inicio de la oleada (baldosas en amarillo + sonido de cronómetro) → identificar la casilla verde adyacente garantizada → desplazarse antes del parpadeo final a rojo letal → avanzar fila por fila hasta pisar la meta (`plataforma`) antes de que expire el reloj general (`0:50`). |
| **Contenido y duración** | Solo tiene un modo. Hasta ahora tiene un solo nivel. La partida dura entre 30 segundos y 50 segundos. |
| **Controles actuales** | Flechas / `WASD`. Movimiento direccional de 4 direcciones (Arriba, Abajo, Izquierda, Derecha). |
| **Pantalla** | Formato 16:9. 1920 × 1080 píxeles. Funciona en pantalla completa. No funciona en ventana. |
| **Assets y licencias** | **Creación Propia (Blender y Paint Tool SAI):**<br>• **Blender 3D:** Modelo 3D del personaje principal, texturizado y animaciones; modelo 3D del policía; modelado de los botones de interfaz y de las baldosas del tablero.<br>• **Paint Tool SAI:** Ilustraciones 2D del abuelo en menús y composición de pantallas.<br><br>**Modelos 3D de Terceros (Sketchfab):**<br>• **Ciudad (`ciudad`):** [Low Poly City](https://sketchfab.com/3d-models/low-poly-city-4a72d39a88264d02af6dde464bbcbb5c).<br>• **Señal vial (`cartel`):** [Road Sign](https://sketchfab.com/3d-models/road-sign-9183f32b83364969bfeb44d2f7d6e935).<br>• **Cartón de leche:** [Low Poly Milk Carton](https://sketchfab.com/3d-models/low-poly-milk-carton-9ccd5400fc6b437abc1e1f0a4d3b5721).<br><br>**Tipografía, Iconos y Fondos 2D:**<br>• **Fuente tipográfica:** [Cute Dino](https://www.dafont.com/cute-dino-2.font), convertida a TextMeshPro SDF con delineado (`Outline + Dilate`) y utilizada en el texto de los botones.<br>• **Iconografía UI:** Símbolo de retorno, señal `"WASD"`, engranaje de opciones y reloj despertador extraídos de Google Imágenes y editados para el proyecto.<br>• **Fondos de pantallas:** Los fondos de las pantallas de victoria, arresto y de las dos páginas de `EscenaIntro` utilizan imágenes extraídas de Google Imágenes combinadas con arte propio.<br><br>**Música y Efectos de Sonido (SFX):**<br>• **Música Menú e Intro (`8bit Bossa`):** [Bossa Nova de OpenGameArt](https://opengameart.org/content/bossa-nova).<br>• **Música Gameplay:** [Kevin MacLeod - Itty Bitty 8 Bit](https://www.youtube.com/watch?v=DzFXGsRvSwA).<br>• **Efectos de Sonido (8 audios):** Obtenidos de [Pixabay Sound Effects](https://pixabay.com/es/sound-effects/search/sfx/) para pasos, baldosa amarilla, baldosa roja, alerta policial, derrota, llegada a tienda, victoria, clic de botón y paso de página. |
| **Estado** | • Flujo completo en PC (`MenuPrincipal` → `EscenaIntro` → `GameplayScene`), mecánicas de grilla, cronómetro con modo pánico, sistema de audio en 3 canales y condiciones de victoria y derrota funcionando en Unity 6.3 (**VERIFICADO**).<br>• Configuración de `Canvas Scaler` en `1920x1080` (**VERIFICADO**).<br>• Bloqueo estricto en orientación Horizontal (*Landscape*), adaptación a controles táctiles, `SafeArea` y expansión de niveles para Android (**PROPUESTO**). |

## Cómo abrir y jugar el proyecto

1. Clonar este repositorio: `git clone https://github.com/ChiaroCoure/MilK-Run-TP-Final.git`
2. Abrir **Unity Hub** y agregar la carpeta del proyecto clonado.
3. Abrir el proyecto utilizando **Unity 6.3 (`6000.3.11f1 LTS`)**.
4. En la ventana *Project*, ir a `Assets/Scenes` (o la carpeta de escenas del proyecto), abrir la escena **`MenuPrincipal`** y presionar **Play** en el editor (asegurarse de que la pestaña *Game* esté en relación de aspecto **16:9 / Full HD 1920x1080**).