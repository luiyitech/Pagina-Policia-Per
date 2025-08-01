document.addEventListener('DOMContentLoaded', function () {

    // --- SLIDER AUTOMÁTICO PARA SERVICIOS (VERSIÓN FINAL CON REACTIVACIÓN) ---
    const serviceTabsContainer = document.querySelector('.service-tabs-container');
    const tabButtons = document.querySelectorAll('.service-tab-button');

    if (serviceTabsContainer && tabButtons.length > 0) {
        if (window.innerWidth >= 992) { // Solo para pantallas de escritorio
            let currentIndex = 0;
            const SLIDE_INTERVAL = 3500; // 3 segundos
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
                }, 3000); // 3000 milisegundos = 5 segundos
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

// --- LÓGICA PARA ACTIVAR PESTAÑAS DESDE EL MENÚ ---
if (window.location.pathname.toLowerCase().includes('institucional')) {
    const hash = window.location.hash;
    if (hash) {
        let tabToActivate = null;
        if (hash === '#mision') {
            tabToActivate = document.querySelector('#mision-tab');
        } else if (hash === '#plana-mayor') {
            tabToActivate = document.querySelector('#plana-mayor-tab');
        }

        if (tabToActivate) {
            const tab = new bootstrap.Tab(tabToActivate);
            tab.show();
        }
    }
}

// --- LÓGICA PARA LA GALERÍA DE IMÁGENES DE LA PÁGINA INSTITUCIONAL ---
$(document).ready(function () {

    // --- LÓGICA PARA LA GALERÍA CON SCROLL INFINITO ---

    // Verificamos si en la página actual existe el contenedor de la galería.
    if ($('#galeria-container').length) {

        let page = 1;
        const pageSize = 6;
        let isLoading = false;
        let noMoreImages = false;
        let initialLoadDone = false;

        // Función principal para cargar imágenes vía AJAX
        function loadMoreImages() {
            if (isLoading || noMoreImages) {
                return;
            }

            isLoading = true;
            $('#loading-spinner').show();

            $.ajax({
                url: '/Institucional/GetGaleriaImages',
                type: 'GET',
                data: { page: page, pageSize: pageSize },
                success: function (images) {
                    if (images && images.length > 0) {
                        images.forEach(function (img) {
                            const imageHtml = `
                                <div class="col-lg-4 col-md-6 gallery-item">
                                    <a href="${img.url}" data-lightbox="institucional-galeria" data-title="${img.title}">
                                        <img src="${img.thumbnailUrl}" class="img-fluid img-thumbnail" alt="${img.altText}">
                                    </a>
                                </div>`;
                            $('#galeria-container').append(imageHtml);
                        });
                        page++;
                    } else {
                        noMoreImages = true;
                        $('#loading-spinner').hide();
                        $('#no-more-images-message').show();
                    }
                },
                error: function () {
                    console.error("Error al cargar las imágenes.");
                    $('#loading-spinner').hide();
                },
                complete: function () {
                    isLoading = false;
                    if (!noMoreImages) {
                        $('#loading-spinner').hide();
                    }
                }
            });
        }

        // Manejo de pestañas para la carga inicial
        const galeriaTabButton = document.querySelector('button[data-bs-target="#galeria-pane"]');
        if (galeriaTabButton) {
            galeriaTabButton.addEventListener('shown.bs.tab', function () {
                if (!initialLoadDone) {
                    loadMoreImages();
                    initialLoadDone = true;
                }
            });
        }

        // Manejo del scroll infinito dentro del contenedor
        $('#galeria-scroll-wrapper').on('scroll', function () {
            const element = this;
            if (element.scrollTop + element.clientHeight >= element.scrollHeight - 100) {
                loadMoreImages();
            }
        });
    } // Cierre del if ('#galeria-container')

    // Configuración global de Lightbox
    // Esta línea debe ejecutarse para que la navegación y el cierre funcionen
    lightbox.option({
        'resizeDuration': 200,
        'wrapAround': true,
        'albumLabel': "Imagen %1 de %2"
    });

}); // Cierre del $(document).ready()

// ===================================================================
//        LÓGICA PARA EL SCROLL INFINITO DE NOTICIAS
// ===================================================================

// Se ejecuta cuando el documento HTML está completamente cargado.
$(document).ready(function () {
    // Verificamos si en la página actual existe el contenedor de noticias.
    // Esto asegura que este código solo se ejecute en la página de "Noticias".
    if ($('#noticias-container').length) {

        let page = 1;
        const pageSize = 3; // Cuántas noticias cargar cada vez.
        let isLoading = false;
        let noMoreNews = false;

        // Función para cargar noticias vía AJAX
        function loadMoreNews() {
            // Si ya estamos cargando o si ya no hay más noticias, no hacemos nada.
            if (isLoading || noMoreNews) {
                return;
            }

            isLoading = true; // Bloqueamos nuevas peticiones.
            $('#loading-spinner-noticias').show(); // Mostramos el ícono de "cargando".

            $.ajax({
                url: '/Noticias/GetNoticias',
                data: { page: page, pageSize: pageSize },
                success: function (noticias) {
                    if (noticias && noticias.length > 0) {
                        noticias.forEach(function (n) {
                            // Construimos el HTML para cada tarjeta de noticia
                            const noticiaHtml = `
                                <div class="col-md-12">
                                    <div class="card noticia-card shadow-sm mb-4">
                                        <div class="row g-0">
                                            <div class="col-md-4">
                                                <img src="${n.imagenUrl}" class="img-fluid rounded-start noticia-img" alt="${n.titulo}">
                                            </div>
                                            <div class="col-md-8">
                                                <div class="card-body d-flex flex-column">
                                                    <h5 class="card-title">${n.titulo}</h5>
                                                    <p class="card-text flex-grow-1">${n.resumen}</p>
                                                    <p class="card-text mt-auto mb-2">
                                                        <small class="text-muted">Publicado: ${n.fecha}</small>
                                                    </p>
                                                    <a href="${n.url}" class="btn btn-primary align-self-start">Leer más...</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;
                            // Añadimos la nueva tarjeta al contenedor
                            $('#noticias-container').append(noticiaHtml);
                        });
                        page++; // Preparamos para la siguiente página
                    } else {
                        // Si el controlador devuelve una lista vacía, significa que no hay más noticias.
                        noMoreNews = true;
                    }
                },
                error: function () {
                    console.error("Error al cargar las noticias.");
                },
                complete: function () {
                    // Cuando la petición termina (sea con éxito o error)...
                    isLoading = false; // ...desbloqueamos para futuras peticiones.
                    $('#loading-spinner-noticias').hide(); // ...y ocultamos el ícono de "cargando".
                }
            });
        }

        // Cargar el primer lote de noticias tan pronto como la página esté lista.
        loadMoreNews();

        // Detectar el scroll para cargar más
        $(window).on('scroll', function () {
            // Se activa cuando el usuario está a 300px (o menos) del final del documento.
            if ($(window).scrollTop() + $(window).height() >= $(document).height() - 300) {
                loadMoreNews();
            }
        });
    }
});