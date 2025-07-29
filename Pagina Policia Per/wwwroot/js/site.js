// Este código se ejecuta una vez que toda la estructura de la página se ha cargado
document.addEventListener('DOMContentLoaded', function () {

    // --- SLIDER AUTOMÁTICO PARA LA SECCIÓN DE SERVICIOS DESTACADOS (VERSIÓN CORREGIDA Y FINAL) ---

    // 1. Seleccionamos los elementos clave
    const serviceTabsContainer = document.querySelector('.service-tabs-container');
    const tabButtons = document.querySelectorAll('.service-tab-button');

    // 2. Verificación de seguridad: Solo continuamos si los elementos existen
    if (serviceTabsContainer && tabButtons.length > 0) {

        // 3. Condición "Solo para PC": Ancho de ventana mayor o igual a 992px
        if (window.innerWidth >= 992) {

            let currentIndex = 0;
            const SLIDE_INTERVAL = 4000; // 4 segundos
            let autoSlideTimer;
            let userHasInteracted = false; // ¡LA BANDERA CLAVE! Inicia en 'false'

            // Función para detener el slider (la llamará una acción del usuario)
            const stopAutoSlide = () => {
                userHasInteracted = true; // El usuario ha tomado el control
                clearInterval(autoSlideTimer); // Detenemos el temporizador para siempre
            };

            // Función que se ejecuta cada 4 segundos
            const goToNextTab = () => {
                // ¡IMPORTANTE! Si el usuario ya interactuó, no hacemos nada más.
                if (userHasInteracted) {
                    return;
                }

                // Calcula el índice de la siguiente pestaña en un bucle infinito
                currentIndex = (currentIndex + 1) % tabButtons.length;

                // Usamos la API de Bootstrap para mostrar la siguiente pestaña
                const nextTab = new bootstrap.Tab(tabButtons[currentIndex]);
                nextTab.show();
            };

            // Función para iniciar el temporizador
            const startAutoSlide = () => {
                // Limpiamos cualquier temporizador anterior por seguridad
                if (autoSlideTimer) {
                    clearInterval(autoSlideTimer);
                }
                // Iniciamos el ciclo que llama a goToNextTab cada 4 segundos
                autoSlideTimer = setInterval(goToNextTab, SLIDE_INTERVAL);
            };

            // 4. Activadores que marcan la interacción del usuario y detienen el slider

            // Si el ratón entra en el área de la sección, se detiene
            serviceTabsContainer.addEventListener('mouseenter', stopAutoSlide);

            // Si el usuario hace clic en CUALQUIER botón, se detiene
            tabButtons.forEach(button => {
                button.addEventListener('click', stopAutoSlide);
            });

            // 5. ¡Iniciamos el slider!
            startAutoSlide();
        }
    }
});