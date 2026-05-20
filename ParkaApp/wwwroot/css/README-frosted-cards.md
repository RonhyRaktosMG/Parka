# Frosted Glass Cards Styling

## Description
Ce fichier CSS contient les styles pour les cartes avec effet "frosted glass" (verre dépoli). 

## Utilisation
Importez le fichier CSS dans votre layout principal:
```html
<link rel="stylesheet" href="~/css/frosted-cards.css" />
```

## Classes CSS Disponibles

### `.card-frosted`
Classe de base pour une carte dépoli verre sans padding.
```html
<div class="card-frosted">Contenu</div>
```

### `.card-frosted-lg`
Carte dépoli verre avec grand padding (24px).
```html
<div class="card-frosted-lg">Contenu</div>
```

### `.card-frosted-md`
Carte dépoli verre avec padding moyen (16px).
```html
<div class="card-frosted-md">Contenu</div>
```

### `.overlay-frosted`
Utilisé pour les overlays ou conteneurs secondaires.
```html
<div class="overlay-frosted">Contenu</div>
```

## Variables CSS
Les couleurs principales sont définies dans les variables CSS:
- `--frosted-bg`: `rgba(255, 255, 255, 0.1)` - Couleur de fond blanc transparent
- `--frosted-border`: `rgba(255, 255, 255, 0.2)` - Couleur de bordure blanche transparente
- `--frosted-blur`: `10px` - Intensité du flou

## Propriétés Principales
- **Fond**: Blanc semi-transparent (10% d'opacité)
- **Bordure**: Blanc semi-transparent (20% d'opacité)
- **Effet Flou**: Arrière-plan flouté pour l'effet verre dépoli
- **Couleur du texte**: Blanc pour contraste optimal

## Modification des Propriétés
Pour modifier les propriétés globales, éditez les variables CSS:
```css
:root {
    --frosted-bg: rgba(255, 255, 255, 0.1);
    --frosted-border: rgba(255, 255, 255, 0.2);
    --frosted-blur: 10px;
}
```

## Compatibilité
- **Navigateurs modernes**: Chrome, Firefox, Safari, Edge
- **Tailwind CSS**: Compatible avec les versions récentes

## Notes
Ces styles remplacent les anciennes cartes avec fond blanc opaque pour un design plus moderne et cohérent avec l'interface sombre de l'application.
