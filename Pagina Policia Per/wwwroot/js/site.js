document.addEventListener('DOMContentLoaded', function () {

    // --- SLIDER AUTOMÁTICO PARA SERVICIOS (VERSIÓN FINAL CON REACTIVACIÓN) ---
    const serviceTabsContainer = document.querySelector('.service-tabs-container');
    const tabButtons = document.querySelectorAll('.service-tab-button');

    if (serviceTabsContainer && tabButtons.length > 0) {
        if (window.innerWidth >= 992) { // Solo para pantallas de escritorio
            let currentIndex = 0;
            const SLIDE_INTERVAL = 4000; // 4 segundos
            let autoSlideTimer; // Temporizador para el ciclo automático
            let restartTimer;   // ¡NUEVO! Temporizador para reiniciar la animación

            // Función que cambia a la siguiente pestaña
            const goToNextTab = () => {
                currentIndex = (currentIndex + 1) % tabButtons.length;
                const nextTab = new bootstrap.Tab(tabButtons[currentIndex]);
                nextTab.show();
            };

            // Función para INICIAR el ciclo de animación
            const startAutoSlide = () => {
                // Limpiamos cualquier temporizador anterior para evitar duplicados
                if (autoSlideTimer) clearInterval(autoSlideTimer);
                if (restartTimer) clearTimeout(restartTimer);

                // Iniciamos el ciclo que cambia de pestaña cada 4 segundos
                autoSlideTimer = setInterval(goToNextTab, SLIDE_INTERVAL);
            };

            // Función para DETENER el ciclo de animación
            const stopAutoSlide = () => {
                clearInterval(autoSlideTimer);
            };

            // --- LA NUEVA LÓGICA PARA REINICIAR ---

            // Función que se activa cuando el ratón SALE del área
            const handleMouseLeave = () => {
                // Limpiamos cualquier temporizador de reinicio que ya estuviera activo
                clearTimeout(restartTimer);
                // Creamos un nuevo temporizador que esperará 5 segundos
                restartTimer = setTimeout(() => {
                    // Después de 5 segundos, reinicia la animación DESDE LA PESTAÑA ACTUAL
                    // Primero, encontramos el índice de la pestaña que está activa en este momento
                    tabButtons.forEach((button, index) => {
                        if (button.classList.contains('active')) {
                            currentIndex = index;
                        }
                    });
                    // Iniciamos el ciclo de nuevo
                    startAutoSlide();
                }, 5000); // 5000 milisegundos = 5 segundos
            };

            // Función que se activa cuando el ratón ENTRA en el área
            const handleMouseEnter = () => {
                // Detenemos el ciclo automático inmediatamente
                stopAutoSlide();
                // Y muy importante: cancelamos cualquier reinicio que estuviera programado
                clearTimeout(restartTimer);
            };

            // Asignamos las funciones a los eventos
            serviceTabsContainer.addEventListener('mouseenter', handleMouseEnter);
            serviceTabsContainer.addEventListener('mouseleave', handleMouseLeave);

            // Si el usuario hace clic, también se detiene el ciclo (y se reiniciará cuando salga)
            tabButtons.forEach(button => {
                button.addEventListener('click', () => {
                    stopAutoSlide();
                    clearTimeout(restartTimer); // Cancelamos el reinicio también al hacer clic
                });
            });

            // ¡Iniciamos la animación por primera vez!
            startAutoSlide();
        }
    }

    // --- INICIALIZACIÓN DEL FONDO ANIMADO DE PARTÍCULAS ---
    if (document.getElementById('plexus-bg')) {
        particlesJS('plexus-bg', {
            "particles": { "number": { "value": 80, "density": { "enable": true, "value_area": 800 } }, "color": { "value": "#ffffff" }, "shape": { "type": "circle" }, "opacity": { "value": 0.5, "random": false }, "size": { "value": 3, "random": true }, "line_linked": { "enable": true, "distance": 150, "color": "#ffffff", "opacity": 0.4, "width": 1 }, "move": { "enable": true, "speed": 2, "direction": "none", "random": false, "straight": false, "out_mode": "out", "bounce": false } },
            "interactivity": { "detect_on": "canvas", "events": { "onhover": { "enable": true, "mode": "grab" }, "onclick": { "enable": true, "mode": "push" }, "resize": true }, "modes": { "grab": { "distance": 140, "line_linked": { "opacity": 1 } }, "push": { "particles_nb": 4 } } },
            "retina_detect": true
        });
    }
});