let totalScore = [];
var search;

document.getElementById("search-button").addEventListener('click', search_item);

function search_item()
{
    search = document.getElementById("nomjson").value;
    fetch('http://yams.iutrs.unistra.fr:3000/api/games/'+search+'/players')
    .then(response => {
        if (!response.ok){ 
            throw new Error('Erreur réseau');
        }
        return response.json(); // Convertir la réponse en JSON
    })
    .then(data => {
        var players = data; // Récupère la liste des joueurs
        const playersList = document.getElementById('players-list');
        playersList.innerHTML='';
    
        // Pour chaque joueur, crée un élément <span> (pas <p>) et ajoute-le à playersList
        players.forEach((player, index) => {
            const playerName = document.createElement('span');
            playerName.textContent = player.pseudo;
            const playerID = player.id;
    
            // Si ce n'est pas le dernier joueur, ajoute une virgule
            if (index < players.length - 1) {
                playerName.textContent += ' ('+playerID+')';
                playerName.textContent += ', ';
            }
            else
            {
                playerName.textContent += ' ('+playerID+')';
            }
            playersList.appendChild(playerName);
        });
    })
}