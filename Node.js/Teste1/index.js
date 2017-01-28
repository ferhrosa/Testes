
var express = require("express");

var app = express();

app.get("/", (req, res) => {
    res.send("Oi mundo!");
})

app.listen(3000);

