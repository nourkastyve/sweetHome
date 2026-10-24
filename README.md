# SweetHome — Application mobile .NET MAUI

SweetHome est un prototype de gestion des commandes d'une pâtisserie.

## Fonctionnalités
- Catalogue de gâteaux avec descriptions et prix en FCFA.
- Commande personnalisée : nom, téléphone, gâteau, taille, message et date de livraison.
- Saisie du prix total et de l'acompte, calcul du reste à payer.
- Suivi : Nouvelle commande, En préparation, Prête, Livrée.
- Historique sauvegardé localement sur l'appareil en JSON.

## Technologies
C#, .NET MAUI, XAML, stockage local JSON.

## Ouvrir
1. Installer Visual Studio avec la charge de travail « Développement .NET Multi-plateforme App UI (.NET MAUI) ».
2. Décompresser le ZIP.
3. Ouvrir `SweetHome.csproj` dans Visual Studio.
4. Choisir une cible disponible (Windows Machine ou émulateur Android), restaurer puis lancer.

**Important :** ce code source doit être compilé et testé dans un environnement avec le SDK .NET MAUI installé. Il n'a pas été compilé ici. Les commandes sont sauvegardées localement sur un appareil, sans serveur partagé.

## Publier sur GitHub
1. Créer un dépôt nommé `SweetHome`.
2. Ouvrir le dépôt, choisir **Add file > Upload files**.
3. Décompresser le ZIP et téléverser les fichiers et dossiers du projet.
4. Cliquer sur **Commit changes**.
