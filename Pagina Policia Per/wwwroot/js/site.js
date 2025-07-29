// Este código se ejecuta una vez que toda la estructura de la página se ha cargado
document.addEventListener('DOMContentLoaded', function () {

    // --- SLIDER AUTOMÁTICO PARA LA SECCIÓN DE SERVICIOS DESTACADOS ---
    const serviceTabsContainer = document.querySelector('.service-tabs-container');
    const tabButtons = document.querySelectorAll('.service-tab-button');

    if (serviceTabsContainer && tabButtons.length > 0) {
        if (window.innerWidth >= 992) {
            let currentIndex = 0;
            const SLIDE_INTERVAL = 4000;
            let autoSlideTimer;
            let userHasInteracted = false;

            const stopAutoSlide = () => {
                userHasInteracted = true;
                clearInterval(autoSlideTimer);
            };

            const goToNextTab = () => {
                if (userHasInteracted) {
                    return;
                }
                currentIndex = (currentIndex + 1) % tabButtons.length;
                const nextTab = new bootstrap.Tab(tabButtons[currentIndex]);
                nextTab.show();
            };

            const startAutoSlide = () => {
                if (autoSlideTimer) {
                    clearInterval(autoSlideTimer);
                }
                autoSlideTimer = setInterval(goToNextTab, SLIDE_INTERVAL);
            };

            serviceTabsContainer.addEventListener('mouseenter', stopAutoSlide);
            tabButtons.forEach(button => {
                button.addEventListener('click', stopAutoSlide);
            });

            startAutoSlide();
        }
    }


    // --- INICIALIZACIÓN DEL FONDO ANIMADO DE PARTÍCULAS (AHORA EN EL LUGAR CORRECTO) ---
    if (document.getElementById('plexus-bg')) {
        particlesJS('plexus-bg', {
            "particles": {
                "number": { "value": 80, "density": { "enable": true, "value_area": 800 } },
                "color": { "value": "#ffffff" },
                "shape": { "type": "circle" },
                "opacity": { "value": 0.5, "random": false },
                "size": { "value": 3, "random": true },
                "line_linked": { "enable": true, "distance": 150, "color": "#ffffff", "opacity": 0.4, "width": 1 },
                "move": { "enable": true, "speed": 2, "direction": "none", "random": false, "straight": false, "out_mode": "out", "bounce": false }
            },
            "interactivity": {
                "detect_on": "canvas",
                "events": {
                    "onhover": { "enable": true, "mode": "grab" },
                    "onclick": { "enable": true, "mode": "push" },
                    "resize": true
                },
                "modes": {
                    "grab": { "distance": 140, "line_linked": { "opacity": 1 } },
                    "push": { "particles_nb": 4 }
                }
            },
            "retina_detect": true
        });
    }

}); // <-- TODO el código ahora está DENTRO de este bloque de cierre.