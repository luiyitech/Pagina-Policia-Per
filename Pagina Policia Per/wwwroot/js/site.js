// Este código se ejecuta una vez que toda la estructura de la página se ha cargado
document.addEventListener('DOMContentLoaded', function () {

    // --- SLIDER AUTOMÁTICO PARA LA SECCIÓN DE SERVICIOS DESTACADOS ---

    // 1. Seleccionamos los elementos con los que vamos a trabajar
    const serviceTabsContainer = document.querySelector('.service-tabs-container');
    const tabButtons = document.querySelectorAll('.service-tab-button');

    // 2. Verificación de seguridad: Solo continuamos si los elementos existen en la página
    if (serviceTabsContainer && tabButtons.length > 0) {

        // 3. Condición "Solo para PC": Verificamos el ancho de la pantalla
        //    Bootstrap considera 'lg' (large) a partir de 992px.
        //    La animación solo se activará si la ventana es más ancha que eso.
        if (window.innerWidth >= 992) {

            let currentIndex = 0; // Para saber qué pestaña está activa
            const SLIDE_INTERVAL = 4000; // 4 segundos
            let autoSlideTimer; // Variable para guardar nuestro temporizador

            // Función que cambia a la siguiente pestaña
            const goToNextTab = () => {
                // Calcula el índice de la siguiente pestaña.
                // El operador % (módulo) hace que vuelva a 0 después de la última pestaña, creando un bucle.
                currentIndex = (currentIndex + 1) % tabButtons.length;

                // Creamos una instancia del componente Tab de Bootstrap para el siguiente botón
                const nextTab = new bootstrap.Tab(tabButtons[currentIndex]);

                // Le decimos a Bootstrap que muestre esa pestaña (esto dispara la animación de fundido)
                nextTab.show();
            };

            // Función para INICIAR el slider automático
            const startAutoSlide = () => {
                // Si ya hay un temporizador, lo limpiamos por seguridad
                if (autoSlideTimer) {
                    clearInterval(autoSlideTimer);
                }
                // Creamos un nuevo temporizador que llama a goToNextTab cada 4 segundos
                autoSlideTimer = setInterval(goToNextTab, SLIDE_INTERVAL);
            };

            // Función para DETENER el slider automático
            const stopAutoSlide = () => {
                clearInterval(autoSlideTimer);
            };

            // 4. Activadores para detener la animación:

            // Si el ratón entra en el área del contenedor de pestañas, se detiene.
            serviceTabsContainer.addEventListener('mouseenter', stopAutoSlide);

            // Si el usuario hace clic en CUALQUIER botón de pestaña, se detiene.
            tabButtons.forEach(button => {
                button.addEventListener('click', stopAutoSlide);
            });

            // 5. ¡Iniciamos el slider!
            startAutoSlide();
        }
    }
});