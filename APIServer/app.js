const express = require("express");
const mongoose = require("mongoose");
const bodyParser = require("body-parser");
const fs = require("fs");
const cors = require("cors");
const Player = require("./models/Player");
const {nanoid} = require("nanoid");

const app = express();
app.use(express.json());
app.use(cors()); //Allows us to make requests from our game.
app.use(bodyParser.json());

const FILE_PATH = "player.json";

//Connection for MongoDB

mongoose.connect("mongodb+srv://LukeDelDeo:Carlymoon1!@cluster0.ia7hq.mongodb.net/GamesDB?retryWrites=true&w=majority&appName=Cluster0");//mongoose.connect("mongodb://localhost:27017/gamedb");

const db = mongoose.connection;

db.on("error", console.error.bind(console, "MongoDB connection error"));
db.once("open", ()=>{
    console.log("Connected to MongoDB Database");
});

//API endpoint for "player.json".

// app.get("/player", (req,res)=>{
//     fs.readFile(FILE_PATH, "utf-8",(err, data)=>{
//         if(err){
//             return res.status(500).json({error:"Unable to fetch data"});
//         }
//         res.json(JSON.parse(data));
//         console.log(`responded with: ${data}`);
//     })
// });

// app.get("/player", async (req, res) => {
//     try {
//         const { playerid, name } = req.query;
//         let query = {};
        
//         if (playerid) {
//             query.playerid = playerid;
//         }
        
//         if (name) {
//             query.name = name;
//         }

//         const players = await Player.find(query);
        
//         if (!players || players.length === 0) {
//             return res.status(404).json({ error: "Player not found" });
//         }

//         res.json(players);
//         console.log(players);
//     } catch (error) {
//         res.status(500).json({ error: "Failed to retrieve players" });
//     }
// });

app.get("/player", async (req, res) => {
    try {
        const players = await Player.find().sort({ name: 1 });  // Sort alphabetically by name
        if (!players) {
            return res.status(404).json({ error: "Players not found" });
        }

        res.json({ players: players });  // Return an array of player objects
    } catch (error) {
        res.status(500).json({ error: "Failed to retrieve players" });
    }
});




app.get("/player/:id", async (req, res) => {
    try {
        console.log("Searching for playerid:", req.params.id); // Debug log

        const player = await Player.findOne({ playerid: req.params.id }); // CORRECT

        if (!player) {
            console.log("Player not found:", req.params.id);
            return res.status(404).json({ error: "Player not found" });
        }

        console.log("Player found:", player);
        res.json(player);
    } catch (error) {
        console.error("Error fetching player:", error);
        res.status(500).json({ error: "Failed to retrieve player" });
    }
});



app.post("/sentdata", (req,res)=>{
    const newPlayerData = req.body;

    console.log(JSON.stringify(newPlayerData,null,2));

    res.json({message:"Player Data recieved"});
});

app.post("/sentdatatodb", async (req,res)=>{
    try{
        const newPlayerData = req.body;

        console.log(JSON.stringify(newPlayerData,null,2));

        const newPlayer = new Player({
            playerid:nanoid(8),
            screenName:newPlayerData.screenName,
            firstName:newPlayerData.firstName,
            lastName:newPlayerData.lastName,
            dateStarted:newPlayerData.dateStarted,
            score:newPlayerData.score
        });
        //Save to database
        await newPlayer.save();
        res.json({message:"Player Added Successfully"});
    }
    catch(error){
        res.status(500).json({error:"Failed to add player",playerid:newPlayer.playerid, name:newPlayer.name});
    }
});

//Update Player
app.post("/updatePlayer", async(req,res)=>{
    const playerData = req.body;

    const player = await Player.findOne({playerid:playerData.playerid})

    if (!player){
        return res.status(404).json({message:"Player not found"});
    }

    player.screenName = playerData.screenName;
    player.firstName = playerData.firstName;
    player.lastName = playerData.lastName;
    player.dateStarted = playerData.dateStarted;
    player.score = playerData.score;

    await player.save();

    res.json({message:"Player updated", player});
});

app.delete("/player/:id", async (req, res) => {
    try {
        console.log("Attempting to delete player with ID:", req.params.id);

        const player = await Player.findOneAndDelete({ playerid: req.params.id });

        if (!player) {
            return res.status(404).json({ error: "Player not found" });
        }

        res.json({ message: "Player deleted successfully", deletedPlayer: player });
    } catch (error) {
        console.error("Error deleting player:", error);
        res.status(500).json({ error: "Failed to delete player" });
    }
});

<<<<<<< Updated upstream

app.listen(3000, ()=>{
    console.log("Running on port 3000");
})
=======
const port = process.env.PORT || 80;
app.listen(port, () => {
    console.log(`Server is running on ${port}`);
 });



>>>>>>> Stashed changes
