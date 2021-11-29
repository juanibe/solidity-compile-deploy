const path = require("path");
const fs = require("fs");
const solc = require("solc");

const contractPath = path.resolve(
  __dirname,
  "../contracts",
  "UsersContract.sol"
);

const source = fs.readFileSync(contractPath, "utf-8");

const input = {
  language: "Solidity",
  sources: {
    "UsersContract.sol": {
      content: source,
    },
  },
  settings: {
    outputSelection: {
      "*": {
        "*": ["*"],
      },
    },
  },
};

module.exports = JSON.parse(solc.compile(JSON.stringify(input)));