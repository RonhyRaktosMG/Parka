(() => {

    const menu = document.getElementById('floatingMenu');
    const button = document.getElementById('menuButton');

    if (menu) {
        menu.style.display = 'none';
    }

    if (button) {
        button.style.display = 'none';
    }

    const messages = [
        'Transformez vos terrains vides en source de revenus',
        'Gérez facilement vos parkings depuis une seule plateforme',
        'Suivez les occupations de parking en temps réel',
        'Digitalisez la gestion de vos terrains de stationnement',
        'Contrôlez les paiements et les occupations simplement',
        'Optimisez l’utilisation de chaque place de parking',
        'Une solution moderne pour gérer vos parkings à Madagascar',
        'Simplifiez l’administration de vos espaces de stationnement',
        'Réduisez le désordre dans la gestion quotidienne des parkings',
        'De terrain vide à parking organisé et rentable'
    ];

    const copy = document.getElementById('hero-copy');

    if (!copy) {
        return;
    }

    let index = 0;

    const showNext = () => {
        copy.style.opacity = '0';
        copy.style.transform = 'translateY(14px)';

        window.setTimeout(() => {
            index = (index + 1) % messages.length;
            copy.textContent = messages[index];
            copy.style.opacity = '1';
            copy.style.transform = 'translateY(0)';
        }, 520);
    };

    window.setInterval(showNext, 4200);
})();