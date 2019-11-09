const Migrations = artifacts.require("Migrations");

const Web3 = require('web3');

module.exports = async function(deployer, network) {
  const web3 = new Web3();

  web3.setProvider(new web3.providers.HttpProvider(`http://${deployer.networks[network].host}:${deployer.networks[network].port}`));

  try {
    await web3.eth.personal.importRawKey("0x642289d27c4126afcabb224c0db945ff7f3e2fc2decebf029cc551bf1e485778", 'password');
  } catch (e) {
    console.log('Private key already imported')
  }
  
  deployer.deploy(Migrations);
};
