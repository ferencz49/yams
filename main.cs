using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



public struct json{
    /*
    Structure : json  
    Description : Contient les différents champs pour l'écriture dans le fichier JSON
    Champs :
    - parameter : Paramètres de la partie
    - player : Tableau qui contient les deux joueurs
    - round : Tableau qui contient les 13 rounds du Yams
    - final_result : Tableau qui contient les résultats finaux pour les deux joueurs
    */
    public parameter parameters;
    public player[] players;
    public round[] rounds;
    public final_result[] results;
    public json(parameter Parameters, player[] Players, round[] Rounds, final_result[] Final_Result){
        parameters = Parameters;
        players = Players;
        rounds = Rounds;
        results = Final_Result;
    }
}

public struct parameter{
    /*
    Structure : parameter  
    Description : Contient les informations générales sur la partie
    Champs :
    - code : Code unique de la partie
    - date : Date à laquelle la partie a été jouée
    */
    public string code;
    public string date;
}

public struct round{
    /*
    Structure : round  
    Description : Représente un round individuel dans la partie
    Champs :
    - id : Identifiant du round
    - result : Tableau qui contient les résultats des deux joueurs pour ce round
    */
    public int id;
    public result[] result;
    public round(int id){
        this.id = id;
        this.result = new result[2];
    }
}

public struct result{
    /*
    Structure : result  
    Description : Contient les informations sur le résultat d'un joueur dans un round
    Champs :
    - id_player : Identifiant du joueur
    - dice : Tableau des valeurs des dés lancés
    - challenge : Code du défi joué lors du round
    - score : Score obtenu pour ce défi
    */
    public int id_player;
    public int[] dice;
    public string challenge;
    public int score;
    public result(int Id_player, string Challenge, int[] Dice, int Score){
        id_player = Id_player;
        dice = Dice;
        challenge = Challenge;
        score = Score;
    }
}

public struct player{
    /*
    Structure : player  
    Description : Représente un joueur dans la partie
    Champs :
    - id : Identifiant du joueur
    - pseudo : Pseudonyme du joueur
    - score_mineur : Score total des défis mineurs
    - score_majeur : Score total des défis majeurs
    - total_score : Score total du joueur (mineur + majeur)
    - challenges : Dictionnaire contenant les défis du joueur
    */
    public int id;
    public string pseudo;
    public int score_mineur;
    public int score_majeur;
    public int total_score;
    public Dictionary<int, chalenge> challenges;

    public player(int Id, string Pseudo){
        pseudo = Pseudo;
        id = Id;
        challenges = new Dictionary<int, chalenge>{
            { 1, new chalenge("nombre1", "Nombre de 1", "Obtenir le maximum de 1") },
            { 2, new chalenge("nombre2", "Nombre de 2", "Obtenir le maximum de 2") },
            { 3, new chalenge("nombre3", "Nombre de 3", "Obtenir le maximum de 3") },
            { 4, new chalenge("nombre4", "Nombre de 4", "Obtenir le maximum de 4") },
            { 5, new chalenge("nombre5", "Nombre de 5", "Obtenir le maximum de 5") },
            { 6, new chalenge("nombre6", "Nombre de 6", "Obtenir le maximum de 6") },
            { 7, new chalenge("chance", "Chance", "Obtenir le maximum de points (le total des dés obtenus)") },
            { 8, new chalenge("petite_suite", "Petite suite", "Obtenir 1-2-3-4 ou 2-3-4-5 ou 3-4-5-6 (30 points)") },
            { 9, new chalenge("grande_suite", "Grande suite", "Obtenir 1-2-3-4-5 ou 2-3-4-5-6 (40 points)") },
            { 10, new chalenge("yams", "Yams", "Obtenir 5 dés de même valeur (50 points)") },
            { 11, new chalenge("brelan", "Brelan", "Obtenir 3 dés de même valeur (Somme des 3 dés identiques)") },
            { 12, new chalenge("carre", "Carré", "Obtenir 4 dés de même valeur (Somme des 4 dés identiques)") },
            { 13, new chalenge("full", "Full", "Obtenir 3 dés de même valeur + 2 dés de même valeur (25 points)") }};
        score_mineur = 0;
        score_majeur = 0;
        total_score = 0;
    }
}

public struct final_result{
    /*
    Structure : final_result  
    Description : Contient les résultats finaux d'un joueur
    Champs :
    - id_player : Identifiant du joueur
    - bonus : Bonus obtenu par le joueur
    - score : Score total du joueur (après application du bonus)
    */
    public int id_player;
    public int bonus;
    public int score;
}

public struct chalenge{
    /*
    Structure : chalenge  
    Description : Représente un défi individuel du jeu
    Champs :
    - code : Code unique du défi
    - name : Nom du défi
    - description : Description du défi
    - complet : Indique si le défi a été complété
    */
    public string code;
    public string name;
    public string description;
    public bool complet;

    public chalenge(string Code, string Name, string Description){
        code = Code;
        name = Name;
        description = Description;
        complet = false;
    }
}


class SAE11{

public static void write_in_file(string texte, StreamWriter sw, int nvindent){
    /*
    Fonction : write_in_file
    Description : Écrit une ligne de texte formatée dans un fichier avec un niveau d'indentation donné.
    Paramètres :
    - texte : Ligne de texte à écrire dans le fichier.
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation (nombre d'espaces à ajouter avant le texte).
    */
    for(int i = 0; i < nvindent; i++){
        sw.Write("  ");
    }
    sw.WriteLine(texte);
}

public static void save_json(json game){
    /*
    Fonction : save_json
    Description : Sauvegarde les données du jeu dans un fichier JSON.
    Paramètres :
    - game : Objet contenant les données du jeu à sauvegarder (paramètres, joueurs, rounds, résultats finaux).
    */
    StreamWriter sw = new StreamWriter("partie.json");
    sw.WriteLine("{");
    int nvindent = 1;
    save_parameter(game.parameters, sw, nvindent);
    save_player(game.players, sw, nvindent);
    save_game(game.rounds, sw, nvindent);
    save_fianl_result(game.results, sw, nvindent);
    sw.WriteLine("}");
    sw.Close();
}

public static void save_parameter(parameter parameters, StreamWriter sw, int nvindent){
    /*
    Fonction : save_parameter
    Description : Sauvegarde les paramètres de la partie dans le fichier JSON.
    Paramètres :
    - parameters : Objet contenant les paramètres du jeu (code et date).
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation actuel pour l'écriture formatée.
    */
    nvindent += 1;
    write_in_file("\"parameters\":{", sw, nvindent);
    write_in_file($"\"code\":\"{parameters.code}\",", sw, nvindent + 1);
    write_in_file($"\"date\":\"{parameters.date}\"", sw, nvindent + 1);
    write_in_file("},", sw, nvindent);
}

public static void save_player(player[] players, StreamWriter sw, int nvindent){
    /*
    Fonction : save_player
    Description : Sauvegarde les données des joueurs dans le fichier JSON.
    Paramètres :
    - players : Tableau contenant les informations des joueurs (id, pseudo, etc.).
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation actuel pour l'écriture formatée.
    */
    sw.WriteLine("\"players\":[");
    nvindent++;
    foreach(player player in players){
        write_in_file("{", sw, nvindent);
        write_in_file($"\"id\":{player.id},", sw, nvindent + 1);
        write_in_file($"\"pseudo\":\"{player.pseudo}\"", sw, nvindent + 1);
        if(player.id != 2){
            write_in_file("},", sw, nvindent);
        } else {
            write_in_file("}", sw, nvindent);
        }
    }
    write_in_file("],", sw, nvindent);
}

public static void save_game(round[] rounds, StreamWriter sw, int nvindent){
    /*
    Fonction : save_game
    Description : Sauvegarde les informations des rounds dans le fichier JSON.
    Paramètres :
    - rounds : Tableau contenant les informations des rounds (id et résultats des joueurs).
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation actuel pour l'écriture formatée.
    */
    write_in_file("\"rounds\":[", sw, nvindent);
    nvindent++;
    foreach(round round in rounds){
        write_in_file("{", sw, nvindent);
        write_in_file($"\"id\":{round.id},", sw, nvindent + 1);
        save_round(round.result, sw, nvindent + 1);
        if(round.id < rounds.Length){
            write_in_file("},", sw, nvindent);
        } else {
            write_in_file("}", sw, nvindent);
        }
    }
    write_in_file("],", sw, nvindent);
}

public static void save_round(result[] results, StreamWriter sw, int nvindent){
    /*
    Fonction : save_round
    Description : Sauvegarde les informations des résultats d'un round dans le fichier JSON.
    Paramètres :
    - results : Tableau contenant les résultats des joueurs pour un round.
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation actuel pour l'écriture formatée.
    */
    write_in_file("\"results\": [", sw, nvindent);
    nvindent++;
    foreach(result result in results){
        write_in_file("{", sw, nvindent);
        write_in_file($"\"id_player\":{result.id_player},", sw, nvindent + 1);
        write_in_file($"\"dice\":[{result.dice[0]},{result.dice[1]},{result.dice[2]},{result.dice[3]},{result.dice[4]}],", sw, nvindent + 1);
        write_in_file($"\"challenge\":\"{result.challenge}\",", sw, nvindent + 1);
        write_in_file($"\"score\":{result.score}", sw, nvindent + 1);
        if(result.id_player != 2){
            write_in_file("},", sw, nvindent);
        } else {
            write_in_file("}", sw, nvindent);
        }
    }
    write_in_file("]", sw, nvindent);
}

public static void save_fianl_result(final_result[] results, StreamWriter sw, int nvindent){
    /*
    Fonction : save_fianl_result
    Description : Sauvegarde les résultats finaux des joueurs dans le fichier JSON.
    Paramètres :
    - results : Tableau contenant les résultats finaux des joueurs (id, bonus, score).
    - sw : StreamWriter utilisé pour écrire dans le fichier.
    - nvindent : Niveau d'indentation actuel pour l'écriture formatée.
    */
    write_in_file("\"final_result\": [", sw, nvindent);
    nvindent++;
    foreach(final_result result in results){
        write_in_file("{", sw, nvindent);
        write_in_file($"\"id_player\":{result.id_player},", sw, nvindent + 1);
        write_in_file($"\"bonus\":{result.bonus},", sw, nvindent + 1);
        write_in_file($"\"score\":\"{result.score}\"", sw, nvindent + 1);
        if(result.id_player != 2){
            write_in_file("},", sw, nvindent);
        } else {
            write_in_file("}", sw, nvindent);
        }
    }
    write_in_file("]", sw, nvindent);
}


    public static void Afficher_Tableau(int[] temp){
        foreach (int t in temp){
            Console.Write(t + " ");
        }
        Console.WriteLine();
    }

    public static void Afficher_dice(int[] dices , int[] keeped_dice){
        int i =0;
        Console.Write("dées :  ");
        foreach (int dice in dices){
        if(keeped_dice[i]==1){
            Console.Write($" [{dice}]");
        }else{
            Console.Write($"   {dice}");   
        }
        i++;
        }
        Console.WriteLine("");
        Console.WriteLine("indice :   0   1   2   3   4");
        Console.WriteLine();
    }

    public static void Afficher_Challenge(Dictionary<int, chalenge> chalenges){
        foreach (KeyValuePair<int, chalenge> elt in chalenges ){
            if(!elt.Value.complet){
            Console.WriteLine($"{elt.Key}) {elt.Value.name}, {elt.Value.description}");  
            }
            else{
                Console.WriteLine($"##{elt.Key}) {elt.Value.name}, {elt.Value.description}##");  
            } 
        }
    }

    public static result Choisir_Challenge(ref player player, int[] dice){
    /*
    Fonction : Choisir_Challenge
    Description : Permet à un joueur de choisir un challenge à réaliser en fonction des dés obtenus et met à jour les scores du joueur.
    Cette fonction demande à l'utilisateur de choisir un challenge, valide l'entrée, calcule le score en fonction des dés, 
    met à jour les scores mineur et majeur du joueur et marque le challenge comme complété.
    
    Paramètres :
    - player : Référence au joueur (avec ses informations comme le score et les challenges disponibles).
    - dice : Tableau des valeurs des dés obtenus lors du lancer.

    Retour :
    - result : Un objet `result` qui contient l'ID du joueur, le code du challenge choisi, les dés utilisés, et le score obtenu.
    */
    
    Console.Write("Choisissez un challenge en entrant son numéro : ");
    int choix = -1;

    // Validation de l'entrée de l'utilisateur
    while (true){
        string input = Console.ReadLine()?.Trim();
        if (int.TryParse(input, out choix) && player.challenges.ContainsKey(choix) && !player.challenges[choix].complet)
        {
            break; // L'entrée est valide
        }
        Console.Write("Entrée invalide. Veuillez entrer un numéro de challenge disponible : ");
    }

    // Déterminer le score pour le challenge choisi
    chalenge challengeChoisi = player.challenges[choix];
    int score = 0;

    // Calcul du score selon le challenge choisi
    switch (challengeChoisi.code){
        case "nombre1":
        case "nombre2":
        case "nombre3":
        case "nombre4":
        case "nombre5":
        case "nombre6":
            int numero = int.Parse(challengeChoisi.code.Replace("nombre", ""));
            score = challenge_mineur(numero, dice);
            player.score_mineur += score; // Mise à jour du score mineur
            break;

        case "chance":
            score = chance(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "petite_suite":
            score = petite_suite(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "grande_suite":
            score = grande_suite(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "yams":
            score = yams(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "brelan":
            score = brelan(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "carre":
            score = carre(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        case "full":
            score = fullhouse(dice);
            player.score_majeur += score; // Mise à jour du score majeur
            break;

        default:
            Console.WriteLine("Erreur : Challenge inconnu.");
            break;
    }

    // Marquer le challenge comme valide
    chalenge tempChallenge = player.challenges[choix];
    tempChallenge.complet = true;
    player.challenges[choix] = tempChallenge;

    // Retourner le résultat pour le round actuel
    return new result(player.id, challengeChoisi.code, dice, score);
    }





    public static int challenge_mineur(int challenge_choisi, int[] resultats_lancés){
    /*
    Fonction : challenge_mineur
    Description : Calcule le score pour un challenge mineur, qui consiste à obtenir un total de dés d'une certaine valeur.
    Cette fonction additionne les valeurs des dés qui correspondent à la valeur spécifiée dans le challenge choisi (par exemple, le total des 1 si "nombre1" est choisi).
    
    Paramètres :
    - challenge_choisi : Entier représentant la valeur du challenge (par exemple, 1 pour "nombre1", 2 pour "nombre2", etc.).
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score total obtenu pour ce challenge mineur, c'est-à-dire la somme des valeurs des dés correspondant à la valeur du challenge choisi.
    */

    int resultat_challenge = 0;

    // Boucle pour parcourir tous les dés et additionner ceux qui correspondent à la valeur du challenge choisi
    for (int i = 0; i < resultats_lancés.Length; i++){
        if (challenge_choisi == resultats_lancés[i]){
            resultat_challenge += resultats_lancés[i]; // Ajoute la valeur du dé au total si elle correspond au challenge
        }
    }

    return resultat_challenge; // Retourne le score total du challenge mineur
    }



    public static int brelan(int[] resultats_lancés){
    /*
    Fonction : brelan
    Description : Calcule le score pour le challenge "Brelan", qui consiste à obtenir trois dés de même valeur.
    Cette fonction vérifie si le joueur a obtenu trois dés identiques. Si c'est le cas, elle retourne la somme des dés identiques multipliée par 3. 
    Sinon, elle retourne 0 pour indiquer qu'il n'y a pas de brelan.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score du brelan, soit 3 fois la valeur des dés identiques si un brelan est obtenu, sinon 0.
    */

    // Trie les dés pour faciliter la recherche de groupes de dés identiques
    Array.Sort(resultats_lancés);

    int somme_couples = 0; // Compteur de dés identiques consécutifs
    int n = 0; // La valeur du dé du brelan trouvé

    // Parcourt les dés triés et vérifie s'il y a deux dés identiques consécutifs
    for (int i = 0; i < resultats_lancés.Length - 1; i++){
        if (resultats_lancés[i] == resultats_lancés[i + 1]){
            n = resultats_lancés[i]; // Met à jour la valeur du dé du brelan trouvé
            somme_couples++; // Augmente le compteur de couples identiques
        }
    }

    // Si moins de 2 couples de dés identiques ont été trouvés, il n'y a pas de brelan
    if (somme_couples < 2){
        return 0; // Pas de brelan, donc score = 0
    }

    // Retourne 3 fois la valeur du dé du brelan
    return 3 * n; // Score du brelan : la valeur du dé multipliée par 3
    }


    public static int carre(int[] resultats_lancés){
    /*
    Fonction : carre
    Description : Calcule le score pour le challenge "Carré", qui consiste à obtenir quatre dés de même valeur.
    Cette fonction vérifie si le joueur a obtenu quatre dés identiques. Si c'est le cas, elle retourne la somme des dés identiques multipliée par 4. 
    Sinon, elle retourne 0 pour indiquer qu'il n'y a pas de carré.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score du carré, soit 4 fois la valeur des dés identiques si un carré est obtenu, sinon 0.
    */

    // Trie les dés pour faciliter la recherche de groupes de dés identiques
    Array.Sort(resultats_lancés);

    int n = 0; // La valeur du dé du carré trouvé
    int somme_couples = 0; // Compteur de dés identiques consécutifs

    // Parcourt les dés triés et vérifie s'il y a des dés identiques consécutifs
    for (int i = 0; i < resultats_lancés.Length - 1; i++){
        if (resultats_lancés[i] == resultats_lancés[i + 1]){
            somme_couples++; // Augmente le compteur de couples identiques
            n = resultats_lancés[i]; // Met à jour la valeur du dé du carré trouvé
        }
    }

    // Si moins de 3 couples de dés identiques ont été trouvés, il n'y a pas de carré
    if (somme_couples < 3){
        return 0; // Pas de carré, donc score = 0
    }

    // Retourne 4 fois la valeur du dé du carré
    return 4 * n; // Score du carré : la valeur du dé multipliée par 4
    }


    public static int fullhouse(int[] resultats_lancés){
    /*
    Fonction : fullhouse
    Description : Calcule le score pour le challenge "Fullhouse", qui consiste à obtenir un full house (une combinaison de trois dés identiques et deux dés identiques).
    Cette fonction vérifie si le joueur a obtenu exactement une combinaison de trois dés de même valeur et deux dés de même valeur.
    Si cette combinaison est présente, elle retourne un score de 25 points. Sinon, elle retourne 0.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score du full house, soit 25 points si un full house est obtenu, sinon 0.
    */

    // La première valeur du tableau est assignée à x, la valeur du premier groupe de dés
    int x = resultats_lancés[0]; 
    int y = 0; // Valeur du second groupe de dés (initialement 0)
    int somme1 = 0; // Compteur pour le groupe x (trois dés identiques)
    int somme2 = 0; // Compteur pour le groupe y (deux dés identiques)

    // Parcours des dés pour déterminer y, la valeur du second groupe de dés
    for (int i = 0; i < resultats_lancés.Length; i++) {
        if (resultats_lancés[i] != x) {
            y = resultats_lancés[i]; // Si le dé n'est pas égal à x, il appartient au second groupe
        }
    }

    // Comptage des dés dans chaque groupe
    for (int i = 0; i < resultats_lancés.Length; i++) {
        if (resultats_lancés[i] == x) {
            somme1 += 1; // Incrémente le compteur pour x
        }
        if (resultats_lancés[i] == y) {
            somme2 += 1; // Incrémente le compteur pour y
        }
    }

    // Si le nombre de dés dans les groupes x et y n'est pas 3 et 2 respectivement, ce n'est pas un full house
    if (somme1 != 3 && somme2 != 2) {
        if (somme2 != 3 && somme1 != 2) {
            return 0; // Pas de full house, donc score = 0
        }
    }

    // Retourne 25 points pour un full house valide
    return 25; 
    }


    public static int petite_suite(int[] resultats_lancés){
    /*
    Fonction : petite_suite
    Description : Calcule le score pour le challenge "Petite suite", qui consiste à obtenir une séquence de quatre dés consécutifs.
    La séquence peut être composée de deux groupes distincts (par exemple 1-2-3-4 ou 2-3-4-5 ou 3-4-5-6).
    Si la séquence est présente, la fonction retourne un score de 30 points. Sinon, elle retourne 0.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score de la petite suite, soit 30 points si une petite suite est obtenue, sinon 0.
    */

    // Trie les dés pour faciliter la recherche de séquences consécutives
    Array.Sort(resultats_lancés);

    int compteur_couples = 0; // Compteur pour les dés qui forment une séquence consécutive

    // Parcourt les dés triés et vérifie si chaque dé forme une séquence consécutive avec le suivant
    for (int i = 0; i < resultats_lancés.Length - 1; i++) {
        if (resultats_lancés[i] + 1 == resultats_lancés[i + 1]) {
            compteur_couples++; // Incrémente le compteur si les dés sont consécutifs
        }
    }

    // Si au moins trois paires consécutives sont trouvées, c'est une petite suite
    if (compteur_couples >= 3) {
        return 30; // Retourne 30 points pour une petite suite valide
    }

    // Si moins de trois paires consécutives sont trouvées, il n'y a pas de petite suite
    return 0; // Pas de petite suite, donc score = 0
    }   


    public static int grande_suite(int[] resultats_lancés){
    /*
    Fonction : grande_suite
    Description : Calcule le score pour le challenge "Grande suite", qui consiste à obtenir une séquence de cinq dés consécutifs.
    La séquence peut être composée de deux groupes distincts (par exemple 1-2-3-4-5 ou 2-3-4-5-6).
    Si la séquence est présente, la fonction retourne un score de 40 points. Sinon, elle retourne 0.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score de la grande suite, soit 40 points si une grande suite est obtenue, sinon 0.
    */

    int compteur_couples = 0; // Compteur pour les dés qui forment une séquence consécutive

    // Trie les dés pour faciliter la recherche de séquences consécutives
    Array.Sort(resultats_lancés);

    // Parcourt les dés triés et vérifie si chaque dé forme une séquence consécutive avec le suivant
    for (int i = 0; i < resultats_lancés.Length - 1; i++) {
        if (resultats_lancés[i] + 1 == resultats_lancés[i + 1]) {
            compteur_couples++; // Incrémente le compteur si les dés sont consécutifs
        }
    }

    // Si cinq dés consécutifs sont trouvés, c'est une grande suite
    if (compteur_couples == 4) {
        return 40; // Retourne 40 points pour une grande suite valide
    }

    // Si moins de cinq paires consécutives sont trouvées, il n'y a pas de grande suite
    return 0; // Pas de grande suite, donc score = 0
    }

    public static int yams(int[] resultats_lancés){
    /*
    Fonction : yams
    Description : Calcule le score pour le challenge "Yams", qui consiste à obtenir cinq dés de même valeur.
    Si tous les dés sont identiques, la fonction retourne un score de 50 points. Sinon, elle retourne 0.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : Le score du Yams, soit 50 points si tous les dés sont identiques, sinon 0.
    */

    int x = resultats_lancés[0]; // Le premier dé est pris comme référence

    // Parcourt les dés et vérifie si tous les dés ont la même valeur que le premier
    for (int i = 1; i < resultats_lancés.Length; i++) {
        if (resultats_lancés[i] != x) {
            return 0; // Si un dé différent est trouvé, retourne 0 (pas de Yams)
        }
    }

    // Si tous les dés sont identiques, retourne 50 points pour le Yams
    return 50; // Score de 50 points pour le Yams
    }


    public static int chance(int[] resultats_lancés){
    /*
    Fonction : chance
    Description : Calcule le score pour le challenge "Chance", qui consiste à additionner tous les dés obtenus lors du lancer.
    Ce challenge donne simplement la somme totale des dés, sans condition particulière.

    Paramètres :
    - resultats_lancés : Tableau contenant les résultats des dés obtenus lors du lancer.

    Retour :
    - int : La somme totale des valeurs des dés, ce qui constitue le score pour le challenge "Chance".
    */

    int resultat = 0; // Variable pour stocker la somme des dés

    // Parcourt le tableau des dés et additionne les valeurs
    for (int i = 0; i < resultats_lancés.Length; i++) {
        resultat += resultats_lancés[i]; // Ajoute chaque dé au total
    }

    // Retourne la somme des dés pour le score "Chance"
    return resultat; // Score obtenu en additionnant les valeurs des dés
    }


    /*
    
    
    
    */
    private static readonly Random rnd = new Random();
    public static int jetter_dé(){
    /*
    Fonction : jetter_dé
    Description : Cette fonction simule le lancement d'un dé à six faces. 
    Retour :
    - int : Un entier représentant la valeur obtenue sur le dé après le lancer, entre 1 et 6.
    */

    int dé = rnd.Next(1, 7); // Génère un nombre aléatoire entre 1 et 6 (inclus)
    return dé; // Retourne la valeur du dé obtenu
    }


public static int[] lancer(){
    /*
    Fonction : lancer
    Description : Cette fonction simule un lancer de 5 dés avec la possibilité de relancer certains dés après chaque tour.
    Le joueur peut choisir quels dés garder entre les tours, et la fonction effectuera jusqu'à deux lancers ou jusqu'à ce que le joueur choisisse de quitter.
    Le joueur peut également entrer 'q' pour quitter à tout moment.

    Paramètres :
    - Aucun paramètre d'entrée.

    Retour :
    - int[] : Un tableau d'entiers représentant les résultats finaux des 5 dés après le dernier lancer (ou après la sortie).
    */

    int[] dice = new int[5] { 0, 0, 0, 0, 0 }; // Tableau pour stocker les résultats des dés
    int[] keeping_dice = new int[5] { 0, 0, 0, 0, 0 }; // Tableau pour marquer les dés à garder (1 = garder, 0 = relancer)
    int nb_tours = 0; // Compteur de tours
    bool quitter = false; // Flag pour indiquer si le joueur veut quitter

    // Boucle principale pour gérer les lancers et les relances
    while (nb_tours < 2 && !quitter){
        // Lancer les dés non gardés
        for (int i = 0; i < 5; i++){
            if (keeping_dice[i] == 0) // Si le dé n'est pas gardé
            {
                dice[i] = jetter_dé(); // Lancer le dé
            }
        }

        // Afficher l'état actuel des dés et des dés à garder
        Afficher_dice(dice, keeping_dice);

        // Réinitialiser le tableau des dés à garder à tous 1 (tout garder)
        keeping_dice = new int[5] { 1, 1, 1, 1, 1 };

        // Demander au joueur quels dés il souhaite garder ou relancer
        Console.Write("Entrer les indices de vos dés à relancer (ex : 0 1 4) OU q pour quitter : ");
        string keep = Console.ReadLine()?.Trim();

        // Si aucune entrée, aucun dé n'est relancé
        if (string.IsNullOrEmpty(keep)) {
            Console.WriteLine("Aucune entrée détectée. Aucun dé ne sera relancé.");
            nb_tours++;
            continue; // Passer au prochain tour sans rien changer
        }

        // Si l'utilisateur entre "q", quitter la boucle
        if (keep.ToLower() == "q"){
            quitter = true;
            break;
        }

        // Séparer les indices des dés à garder
        string[] keeps = keep.Split(" "); 

        bool validInput = false; // Variable pour contrôler si l'entrée est valide

        // Redemander les indices si l'entrée est invalide
        while (!validInput)
        {
            validInput = true; // Supposer que l'entrée est valide

            foreach (string value_to_keep in keeps)
            {
                try
                {
                    int indice = int.Parse(value_to_keep);

                    // Vérifier si l'indice est valide (entre 0 et 4)
                    if (indice >= 0 && indice < 5)
                    {
                        keeping_dice[indice] = 0; // Marquer ce dé pour le relancer
                    }
                    else
                    {
                        Console.WriteLine($"{value_to_keep} : indice hors limites (doit être entre 0 et 4).");
                        validInput = false;
                        break; // Redemander l'entrée si l'indice est hors limites
                    }
                }
                catch (FormatException)
                {
                    // Gérer les erreurs si l'entrée n'est pas un nombre entier
                    Console.WriteLine($"{value_to_keep} : valeur invalide (doit être un entier).");
                    validInput = false;
                    break; // Redemander l'entrée en cas d'erreur de format
                }
            }

            // Si l'entrée est invalide, demander à nouveau les indices
            if (!validInput)
            {
                Console.Write("Entrer à nouveau les indices de vos dés à relancer (ex : 0 1 4) OU q pour quitter : ");
                keep = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(keep) || keep.ToLower() == "q")
                {
                    quitter = true;
                    break;
                }
                keeps = keep.Split(" "); // Réparer la nouvelle entrée
            }
        }

        nb_tours++; // Incrémenter le nombre de tours
    }

    // Si le joueur n'a pas quitté, effectuer un dernier lancer des dés non gardés
    if (!quitter)
    {
        for (int i = 0; i < 5; i++)
        {
            if (keeping_dice[i] == 0){
                dice[i] = jetter_dé(); // Relancer les dés non gardés
            }
        }
    }

    return dice; // Retourner les résultats finaux des dés après les relances
}






public static void Main(){
    // Initialisation des variables
    round[] game = new round[13]; // Tableau de 13 rounds (un pour chaque challenge)
    player[] players = new player[2]; // Tableau pour les 2 joueurs
    parameter parameters = new parameter(); // Paramètres de configuration du jeu
    final_result[] results = new final_result[2]; // Tableau pour stocker les résultats finaux des joueurs
    parameters.code = "groupe34"; // Code du groupe de développement
    parameters.date = DateTime.Now.ToString("MM/dd/yyyy"); // Date du jour

    // Demander aux joueurs de saisir leur pseudo
    Console.WriteLine("Joueur 1 : Rentrez votre pseudo : ");
    players[0] = new player(1, Console.ReadLine()); // Création du premier joueur
    Console.WriteLine("Joueur 2 : Rentrez votre pseudo : ");
    players[1] = new player(2, Console.ReadLine()); // Création du deuxième joueur

    // Initialisation des 13 rounds
    for (int i = 0; i < game.Length; i++){
        game[i] = new round(i + 1); // Crée un round pour chaque challenge
    }

    // Boucle principale du jeu (13 tours)
    for(int i = 0; i < 13; i++){
        // Tour de chaque joueur
        for (int j = 0; j < players.Length; j++){
            player currentPlayer = players[j]; // Référence au joueur actuel
            Console.WriteLine($"{currentPlayer.pseudo}, c'est à toi de jouer "); // Annonce du joueur
            int[] dés = lancer(); // Lancer les dés
            Console.WriteLine(""); 
            Console.Write("Vos dés finaux sont : ");
            Afficher_Tableau(dés); // Affiche les résultats des dés
            Console.WriteLine("");
            Console.WriteLine($"Point Total : {currentPlayer.score_majeur + currentPlayer.score_mineur}, Somme mineure : {currentPlayer.score_mineur}");
            Afficher_Challenge(currentPlayer.challenges); // Affiche les challenges disponibles
            result playerturn = Choisir_Challenge(ref currentPlayer, dés); // Le joueur choisit un challenge et modifie ses scores
            players[j] = currentPlayer; // Met à jour les statistiques du joueur
            game[i].result[currentPlayer.id - 1] = playerturn; // Stocke le résultat du tour pour ce joueur
        }
    }

    // Calcul du score total de chaque joueur après les 13 rounds
    players[0].total_score = players[0].score_mineur + players[0].score_majeur;
    if (players[0].score_mineur >= 65) { // Bonus de 35 points si la somme des scores mineurs est >= 65
        players[0].total_score += 35;
        results[0].bonus = 35;
    }

    players[1].total_score = players[1].score_mineur + players[1].score_majeur;
    if (players[1].score_mineur >= 65) { // Bonus de 35 points pour le deuxième joueur aussi
        players[1].total_score += 35;
        results[1].bonus = 35;
    }

    // Comparaison des scores finaux pour déterminer le gagnant
    if (players[0].total_score > players[1].total_score) {
        Console.WriteLine($"Bravo : {players[0].pseudo} tu as gagné , avec {players[0].total_score}");
    } else if (players[0].total_score < players[1].total_score) {
        Console.WriteLine($"Bravo : {players[1].pseudo} tu as gagné , avec {players[1].total_score}");
    } else {
        Console.WriteLine("Égalité");
    }

    // Enregistrer les résultats finaux
    results[0].id_player = players[0].id;
    results[0].score = players[0].total_score;
    results[1].id_player = players[1].id;
    results[1].score = players[1].total_score;

    // Créer un objet JSON pour enregistrer les résultats du jeu
    json final_json = new json(parameters, players, game, results);
    save_json(final_json); // Sauvegarder les résultats dans un fichier JSON
    }
}
