$(document).ready(function () {
    $('.btn-filter').click(function () {
        var filterValue = $(this).attr('data-filter');

        $('.btn-filter').removeClass('active');
        $(this).addClass('active');

        if (filterValue === 'all') {
            $('.produto-item').show();
        } else {
            $('.produto-item').hide();

            $('.produto-item[data-categoria="' + filterValue + '"]')
                .show();
        }
    });

    $('.cat-list li a').click(function (e) {
        e.preventDefault();

        var cat = $(this).data('filter-cat');

        if (cat) {
            $('.btn-filter').removeClass('active');

            $('.btn-filter[data-filter="' + cat + '"]')
                .addClass('active');

            $('.produto-item').hide();

            $('.produto-item[data-categoria="' + cat + '"]')
                .show();
        }
    });

    $('#search-btn').click(function () {
        performSearch();
    });

    $('#search-input').keypress(function (e) {
        if (e.which === 13) {
            performSearch();
        }
    });

    function performSearch() {
        var term = $('#search-input')
            .val()
            .toLowerCase();

        if (term === '') {
            var activeFilter = $('.btn-filter.active')
                .data('filter');

            if (activeFilter === 'all') {
                $('.produto-item').show();
            } else {
                $('.produto-item').hide();

                $('.produto-item[data-categoria="' + activeFilter + '"]')
                    .show();
            }
        } else {
            $('.produto-item').each(function () {
                var nome = $(this)
                    .data('nome')
                    .toLowerCase();

                if (nome.indexOf(term) !== -1) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        }
    }

    function atualizarTotal(card) {
        const preco = parseFloat(
            card.dataset.produtoPreco
        );

        const campoQuantidade = card.querySelector(
            '.quantidade-produto'
        );

        const elementoTotal = card.querySelector(
            '.valor-total'
        );

        if (
            !campoQuantidade ||
            !elementoTotal ||
            Number.isNaN(preco)
        ) {
            return;
        }

        let quantidade = parseInt(
            campoQuantidade.value,
            10
        );

        if (
            Number.isNaN(quantidade) ||
            quantidade < 1
        ) {
            quantidade = 1;
            campoQuantidade.value = 1;
        }

        const total = preco * quantidade;

        elementoTotal.textContent = total.toLocaleString(
            'pt-BR',
            {
                style: 'currency',
                currency: 'BRL'
            }
        );
    }

    document
        .querySelectorAll('.single-produto-card')
        .forEach(function (card) {
            const campoQuantidade = card.querySelector(
                '.quantidade-produto'
            );

            if (!campoQuantidade) {
                return;
            }

            campoQuantidade.addEventListener(
                'input',
                function () {
                    atualizarTotal(card);
                }
            );

            atualizarTotal(card);
        });
});