document.addEventListener('DOMContentLoaded', function () {
    const botaoWhatsApp =
        document.getElementById('botao-whatsapp');

    const campoNome =
        document.getElementById('nome-whatsapp');

    const campoMensagem =
        document.getElementById('mensagem-whatsapp');

    const mensagemValidacao =
        document.getElementById('mensagem-validacao');

    botaoWhatsApp.addEventListener('click', function () {
        const nome = campoNome.value.trim();
        const mensagem = campoMensagem.value.trim();

        if (nome === '' || mensagem === '') {
            mensagemValidacao.textContent =
                'Preencha seu nome e sua mensagem antes de continuar.';

            mensagemValidacao.style.display = 'block';
            return;
        }

        mensagemValidacao.style.display = 'none';

        const textoWhatsApp =
            'Nome: ' + nome +
            '\n\nMensagem:\n' + mensagem;

        const numeroWhatsApp = '5514997354643';

        const enderecoWhatsApp =
            'https://wa.me/' +
            numeroWhatsApp +
            '?text=' +
            encodeURIComponent(textoWhatsApp);

        navigator.sendBeacon('/Home/RegistrarContatoWhatsApp');

        window.open(enderecoWhatsApp, '_blank');
    });
}); 