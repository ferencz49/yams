// on etabli un dictionnaire scores que l'on va exploiter dans le code plus tard
let scores = {
    "nombre1" : 0,
    "nombre2" : 0,
    "nombre3" : 0,
    "nombre4" : 0,
    "nombre5" : 0,
    "nombre6" : 0,
    "bonus" : 0,
    "brelan" : 0,
    "carre" : 0,
    "full" : 0,
    "petite_suite" : 0,
    "grande_suite" : 0,
    "yams" : 0,
    "chance" : 0,
    "total" : 0
};

let gameLoading = false;

// on charge les tableaux à vide (pour les afficher)
createTab(1);
createTab(2);

document.getElementById("search-button").addEventListener('click', ShowGame);

// Fonction asynchrome, celle-ci pourras utilliser le 'await'
async function ShowGame()
{
    // --- DESCRIPTION --- //
    // Cette fonction sert à afficher les scores finaux avec les challenges choisis par les joueurs et 
    // les points obtenus pour chaque challenge.
    // --- //

    if (!gameLoading) {
        gameLoading = true;
        let search = document.getElementById("nomjson").value;
        let response = await fetch('http://yams.iutrs.unistra.fr:3000/api/games/'+search+'/rounds/1') // on effectue un teste, pour voir si l'ID de la partie est valide
        if (!response.ok) {
            //Code pour gérer le cas où la réponse n'est pas correcte
            throw window.alert(["Veuillez mettre un ID de partie valide et appuyez sur 'Rechercher'!"]);//Le fetch s'arrête !
        }
        else{
            // Pour chaque joueur (2 dans notre cas), on va chercher son score du challenge de chaque round et les afficher dans un tableau
            for (let y = 1; y < 3; y++) {
                await setScores(search, y); // on cherche les scores pour le joueur y
                createTab(y); // on crée le tableau pour le joueur y (1 ou 2)
                resetScores(); // on remet les scores à 0 pour le joueur y
            }
        }
        gameLoading = false;
    }
    else
    {
        window.alert(["Patientez le temps que les données soient chargées!"]);
    }
}

function createTab(player)
{
    // --- DESCRIPTION --- //
    // Cette fonction sert a actualiser les données dans le tableau des scores
    // finaux du jeu de yams pour le joueur dans la variable 'player'
    // - player est un entier qui contient le numéro du joueur (1 ou 2)
    // --- //

    let gameTable = document.getElementById('scoreTable'+player);
    
    // Vider le tableau au cas ou il y aurait deja des données
    gameTable.innerHTML = '';

    // Parcours du dictionaire scores pour rajouter une ligne pour chaque challenge
    for (let challenge in scores) {
        // Créer une nouvelle ligne <tr>
        let row = document.createElement('tr');
        
        // créer la colonne pour le challenge
        let challengeCell = document.createElement('td');
        challengeCell.textContent = challenge;
        if (challenge === 'bonus') { // si le (potentiel) bonus "bonus" est bien > 63, on le met en vert pour montrer qu'on a un bonus
            if (scores['bonus'] > 63) {
                challengeCell.style.backgroundColor = 'green';
            }
        }
        row.appendChild(challengeCell);

        // créer la colonne pour le score
        let scoreCell = document.createElement('td');
        scoreCell.textContent = scores[challenge];
        row.appendChild(scoreCell);

        // ajouter la ligne à la table
        gameTable.appendChild(row);
    }
}

// le await sera principalement utilisé ici, et cette fonction sera appelée dans la fonction ShowGame
async function setScores(search, playerID)
{
    // --- DESCRIPTION --- //
    // Cette fonction sert à chercher les scores pour chaque joueur et les ajouter dans le dictionnaire 'scores'
    // - search est un entier qui contient l'ID de la partie
    // - playerID est un entier qui contient le numéro du joueur (1 ou 2)
    // --- //

    // Pour chaque round, on va chercher les données correspondantes à ce round et on va les ajouter dans le dictionnaire 'scores'
    for (let i = 1; i < 14; i++) {
        let response = await fetch('http://yams.iutrs.unistra.fr:3000/api/games/'+search+'/rounds/'+i)
        try {
            response = await response.json(); // Convertir la réponse en JSON
        }
        catch (err) {
            console.log(err); // si jamais il y a une erreur, voir dans la console
        }
        // affecter les scores dans le dictionnaire 'scores'
        let data = response
        // Parcours chaque résultat dans "results"
        data.results.forEach(player => {
            // on vérifie si c'est bien le joueur que l'on veut (y)
            if (player.id_player == playerID) {
                // ajouter ce score dans le dictionnaire 'scores' avec le challenge correspondant
                scores[player.challenge] = player.score;
                for (let k = 1; k < 7; k++) // on verifie si le challenge est un challenge mineur
                {                           // et si oui on ajoute le score dans le (potentiel) bonus 'bonus' 
                    if (player.challenge === "nombre"+k) {
                        scores["bonus"] += player.score;
                    }
                }
            }
        });
    }

    // Ensuite on va récuperer les scores finaux
    let response = await fetch('http://yams.iutrs.unistra.fr:3000/api/games/'+search+'/final-result')
    response = await response.json(); // Convertir la réponse en JSON
    let data = response;
    data.forEach(player => {
        // on vérifie si c'est bien le joueur que l'on veut (y)
        if (player.id_player == playerID) {
            // ajouter ce score dans le dictionnaire 'scores' avec le challenge correspondant
            scores["total"] = player.score;
        }
    });
}

function resetScores()
{
    // --- DESCRIPTION --- //
    // Cette fonction sert à remettre les scores à 0
    // --- //

    for (let challenge in scores) {
        scores[challenge] = 0;
    }
}