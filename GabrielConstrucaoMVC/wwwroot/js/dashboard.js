document.addEventListener('DOMContentLoaded', function () {
    const seletor = document.getElementById('periodo-faturamento');
    const canvas = document.getElementById('grafico-faturamento');
    const totalElemento =
        document.getElementById('total-faturamento-periodo');

    if (!seletor || !canvas || !totalElemento) {
        return;
    }

    const copiaNiceSelect = seletor.nextElementSibling;

    if (
        copiaNiceSelect &&
        copiaNiceSelect.classList.contains('nice-select')
    ) {
        copiaNiceSelect.remove();
    }

    seletor.style.display = 'block';

    const dadosFaturamento = window.dashboardDados;

    function formatarMoeda(valor) {
        return valor.toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        });
    }

    function desenharMensagem(
        contexto,
        largura,
        altura,
        mensagem
    ) {
        contexto.fillStyle = '#7b8491';
        contexto.font = '14px Arial';
        contexto.textAlign = 'center';

        contexto.fillText(
            mensagem,
            largura / 2,
            altura / 2
        );
    }

    function desenharGrafico(periodo) {
        const dados = dadosFaturamento[periodo];

        if (!dados) {
            return;
        }

        const rotulos = Array.isArray(dados.rotulos)
            ? dados.rotulos
            : [];

        const valores = Array.isArray(dados.valores)
            ? dados.valores.map(Number)
            : [];

        const quantidadeDados = Math.min(
            rotulos.length,
            valores.length
        );

        const rotulosValidos =
            rotulos.slice(0, quantidadeDados);

        const valoresValidos =
            valores.slice(0, quantidadeDados);

        const total = valoresValidos.reduce(
            function (soma, valor) {
                return soma + valor;
            },
            0
        );

        totalElemento.textContent = formatarMoeda(total);

        const contexto = canvas.getContext('2d');

        if (!contexto) {
            return;
        }

        const container = canvas.parentElement;

        const largura = Math.max(
            container.clientWidth,
            300
        );

        const altura = Math.max(
            container.clientHeight,
            250
        );

        const proporcao =
            window.devicePixelRatio || 1;

        canvas.width = largura * proporcao;
        canvas.height = altura * proporcao;

        canvas.style.width = largura + 'px';
        canvas.style.height = altura + 'px';

        contexto.setTransform(
            proporcao,
            0,
            0,
            proporcao,
            0,
            0
        );

        contexto.clearRect(
            0,
            0,
            largura,
            altura
        );

        if (quantidadeDados === 0) {
            desenharMensagem(
                contexto,
                largura,
                altura,
                'Nenhum faturamento registrado no período.'
            );

            return;
        }

        const margemEsquerda = 75;
        const margemDireita = 25;
        const margemSuperior = 30;
        const margemInferior = 45;

        const larguraGrafico =
            largura - margemEsquerda - margemDireita;

        const alturaGrafico =
            altura - margemSuperior - margemInferior;

        const maiorValor =
            Math.max(...valoresValidos, 1);

        contexto.strokeStyle = '#e7ebef';
        contexto.lineWidth = 1;

        for (let linha = 0; linha <= 4; linha++) {
            const y =
                margemSuperior +
                (alturaGrafico / 4) * linha;

            contexto.beginPath();

            contexto.moveTo(
                margemEsquerda,
                y
            );

            contexto.lineTo(
                largura - margemDireita,
                y
            );

            contexto.stroke();

            const valorLinha =
                maiorValor -
                (maiorValor / 4) * linha;

            contexto.fillStyle = '#8993a1';
            contexto.font = '11px Arial';
            contexto.textAlign = 'right';

            contexto.fillText(
                formatarMoeda(valorLinha),
                margemEsquerda - 8,
                y + 4
            );
        }

        const intervaloRotulos =
            rotulosValidos.length > 15
                ? Math.ceil(rotulosValidos.length / 10)
                : 1;

        const divisor =
            rotulosValidos.length > 1
                ? rotulosValidos.length - 1
                : 1;

        const pontos = valoresValidos.map(
            function (valor, indice) {
                const x =
                    rotulosValidos.length === 1
                        ? margemEsquerda + larguraGrafico / 2
                        : margemEsquerda +
                        (larguraGrafico / divisor) * indice;

                const y =
                    margemSuperior +
                    alturaGrafico -
                    (valor / maiorValor) * alturaGrafico;

                return {
                    x: x,
                    y: y
                };
            }
        );

        if (pontos.length > 0) {
            const degrade =
                contexto.createLinearGradient(
                    0,
                    margemSuperior,
                    0,
                    margemSuperior + alturaGrafico
                );

            degrade.addColorStop(
                0,
                'rgba(255, 95, 19, 0.20)'
            );

            degrade.addColorStop(
                1,
                'rgba(255, 95, 19, 0.02)'
            );

            contexto.beginPath();

            contexto.moveTo(
                pontos[0].x,
                margemSuperior + alturaGrafico
            );

            pontos.forEach(function (ponto) {
                contexto.lineTo(
                    ponto.x,
                    ponto.y
                );
            });

            contexto.lineTo(
                pontos[pontos.length - 1].x,
                margemSuperior + alturaGrafico
            );

            contexto.closePath();
            contexto.fillStyle = degrade;
            contexto.fill();
        }

        contexto.beginPath();
        contexto.strokeStyle = '#ff5f13';
        contexto.lineWidth = 3;
        contexto.lineJoin = 'round';
        contexto.lineCap = 'round';

        pontos.forEach(function (ponto, indice) {
            if (indice === 0) {
                contexto.moveTo(
                    ponto.x,
                    ponto.y
                );
            } else {
                contexto.lineTo(
                    ponto.x,
                    ponto.y
                );
            }
        });

        contexto.stroke();

        pontos.forEach(function (ponto, indice) {
            contexto.beginPath();
            contexto.fillStyle = '#ffffff';
            contexto.strokeStyle = '#ff5f13';
            contexto.lineWidth = 3;

            contexto.arc(
                ponto.x,
                ponto.y,
                5,
                0,
                Math.PI * 2
            );

            contexto.fill();
            contexto.stroke();

            const mostrarRotulo =
                indice % intervaloRotulos === 0 ||
                indice === rotulosValidos.length - 1;

            if (mostrarRotulo) {
                contexto.fillStyle = '#7b8491';
                contexto.font = '11px Arial';
                contexto.textAlign = 'center';

                contexto.fillText(
                    rotulosValidos[indice],
                    ponto.x,
                    altura - 17
                );
            }
        });
    }

    seletor.addEventListener(
        'change',
        function () {
            desenharGrafico(seletor.value);
        }
    );

    let temporizadorRedimensionamento;

    window.addEventListener(
        'resize',
        function () {
            clearTimeout(temporizadorRedimensionamento);

            temporizadorRedimensionamento = setTimeout(
                function () {
                    desenharGrafico(seletor.value);
                },
                150
            );
        }
    );

    desenharGrafico(
        seletor.value || 'seteDias'
    );
});