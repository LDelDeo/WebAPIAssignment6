const mongoose = require("mongoose");
const Player = require("./models/Player");
const {nanoid} = require("nanoid");

mongoose.connect("mongodb+srv://LukeDelDeo:Carlymoon1!@cluster0.ia7hq.mongodb.net/GamesDB?retryWrites=true&w=majority&appName=Cluster0");

async function addPlayer(){
    await Player.create({
        playerid:nanoid(8),
        screenName:"Jay",
        firstName:"Jason",
        lastName:"V",
        dateStarted:"2/3/23",
        score:2
    });

    console.log("Player Added");
    mongoose.connection.close();
}

addPlayer();