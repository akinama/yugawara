const Commitment = artifacts.require("Commitment");

module.exports = function(deployer) {
    deployer.deploy(Commitment);
};
