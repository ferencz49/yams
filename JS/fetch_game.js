var currentRound = 1;
var search;
var showingGame = false;
refresh_button();

document.getElementById("search-button").addEventListener('click', search_item)

document.querySelector('.goBack').addEventListener('click', function() {
    currentRound--;
    if (currentRound<=0)
    {
        currentRound=1;
    }
    ShowGame(currentRound, search);
    refresh_button();
});

document.querySelector('.goForward').addEventListener('click', function() {
    currentRound++;
    if (currentRound>=14)
    {
        currentRound=13;
    }
    ShowGame(currentRound, search);
    refresh_button();
});

function refresh_button(){
    // Fonction pour changer les boutons de changement de rounds (si jamais on est au premier ou dernier round)
    let button = document.getElementsByClassName('goBack')[0];
    if (showingGame)
    {
        if (currentRound <= 1)
        {
            button.style.backgroundColor = "Gray";
        }
        else
        {
            button.style.backgroundColor = "Beige"
        }
        button = document.getElementsByClassName('goForward')[0];
        if (currentRound >= 13)
        {
            button.style.backgroundColor = "Gray";
        }
        else
        {
            button.style.backgroundColor = "Beige"
        }
    }
    else
    {
        button.style.backgroundColor = "Gray";
        button = document.getElementsByClassName('goForward')[0];
        button.style.backgroundColor = "Gray";
    }
}

function search_item()
{
    search = document.getElementById("nomjson").value;
    ShowGame(currentRound, search);
}

function ShowGame(currentRound, search)
{
    // --- DESCRIPTION --- //
    // Cette fonction sert à afficher le jeu en entier avec tous les joueurs individuellement.
    // currentRound est un entier qui définit le round actuel, on prends en compte que le currentRound
    // est une valeur valide (entre 1 et 13) car elle est vérifiée avant de lancer la fonction.
    // ---

    fetch('http://yams.iutrs.unistra.fr:3000/api/games/'+search+'/rounds/'+currentRound)
    .then(response => {
        if (!response.ok) {
            //Code pour gérer le cas où la réponse n'est pas correcte
            throw window.alert(["Veuillez mettre un ID de partie VALABLE et appuyez sur 'Rechercher'!"]);//Le fetch s'arrête !
        }
        return response.json(); // Convertir la réponse en JSON
    })
    // AFFICHER LES DONNEES
    .then(data => {
        showingGame = true;
        refresh_button();
        let round = data; // Récupère les données correspondant au round actuel
        const gameDisplay = document.getElementById('game-show');
        const gameDisplay2 = document.getElementById('player-games');
        
        gameDisplay.innerHTML = ''; // vide le contenu de game-show
        gameDisplay2.innerHTML = ''; // vide le contenu de game-show

        // affiche 'Round ...'
        const roundText = document.createElement('h2');
        roundText.textContent = 'Round ' + currentRound;
        gameDisplay.appendChild(roundText);

        // afficher le jeu des joueurs
        data.results.forEach(player => {
            // On va créer une div pour chaque joueur pour les séparer, ceci offre un moyen d'ajouter
            // plus de 2 joueurs aussi
            const gameDisplay2 = document.getElementById('player-games');

            let divplayer = document.createElement('div'); // on crée une div pour grouper tous les éléments du joueur
            divplayer.className = "divPlayer";
            gameDisplay2.appendChild(divplayer); // de cette façon, on peut afficher les dés et les challenges l'un en dessous de l'autre

            let jeu1 = document.createElement('h3');
            jeu1.textContent = "Joueur "+player.id_player+":"; // Afficher "Joueur 1:" (exemple)
            divplayer.appendChild(jeu1); 
            
            let divplayer2 = document.createElement('div'); // div pour les dés
            divplayer2.className = "divPlayer2";
            divplayer.appendChild(divplayer2);

            // AFFICHER LES DÉS (en image)
            player.dice.forEach(de => {
                let imgdice = document.createElement("img");
                imgdice.src = "Images/dice"+de+".png";
                divplayer2.appendChild(imgdice);
            });

            let divplayer3 = document.createElement('div'); // div pour les le challenge choisi
            divplayer3.className = "divPlayer2";
            divplayer.appendChild(divplayer3);

            let jeu2 = document.createElement('p');
            jeu2.textContent = "Challenge choisi: "+player.challenge; // Afficher "Challenge choisi: brelan" (exemple)
            divplayer3.appendChild(jeu2);

            let divplayer4 = document.createElement('div'); // div pour le score
            divplayer4.className = "divPlayer2";
            divplayer.appendChild(divplayer4);

            let jeu3 = document.createElement('p');
            jeu3.textContent = "Score: "+player.score; // Afficher "Score: 25" (exemple)
            divplayer4.appendChild(jeu3);
        });
    })
}