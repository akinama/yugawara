pragma solidity ^0.5.2;

import '@openzeppelin/contracts/payment/escrow/Escrow.sol';

contract Commitment {
    Escrow private _escrow;
    
    event Commitment(address indexed Address, address indexed TrainerAddress, uint256 Deposit);
    event Deposited(address indexed projectOwner, uint256 weiAmount);
    event Recorded(address indexed projectOwner, uint index, uint256 record);
    event Completed(address indexed projectOwner, uint index);
    event Withdrawn(address indexed projectOwner, address indexed trainer, uint256 weiAmount);

    struct Project {
        uint count;
        address[] trainers;
        uint256[][] records;
        uint256[][] recordedTimes;
        uint256[] currentAmounts;
        bool[] completions;
        bool[] hasBeenWithdrawn;
    }

    mapping(address => Project) private _projects;

    constructor () public {
        _escrow = new Escrow();
    }

    modifier onlyDepositable (uint index) {
        require(!_projects[msg.sender].hasBeenWithdrawn[index]);

        _;
    }

    modifier onlyWithdrawable (uint index) {
        require(!_projects[msg.sender].hasBeenWithdrawn[index]);

        require(_projects[msg.sender].completions[index]);

        _;
    }

    modifier onlyTrainerOfProject (address projectOwner, uint index) {
        require(_projects[projectOwner].trainers[index] == msg.sender);

        require(!_projects[projectOwner].completions[index]);

        _;
    }

    function addProject(address trainer) public {
        _projects[msg.sender].count++;
        _projects[msg.sender].trainers.push(trainer);
        _projects[msg.sender].currentAmounts.push(0);
        _projects[msg.sender].completions.push(false);
        _projects[msg.sender].hasBeenWithdrawn.push(false);
        
        emit Commitment(msg.sender, trainer, 0);
    }

    function depositByIndex(uint index) public payable onlyDepositable(index) {
        _projects[msg.sender].currentAmounts[index] += msg.value;

        _escrow.deposit(msg.sender);

        emit Deposited(msg.sender, msg.value);
    }

    function recordByIndex(uint256 record, uint index) public onlyDepositable(index) {
        _projects[msg.sender].records[index].push(record);
        _projects[msg.sender].recordedTimes[index].push(block.timestamp);

        emit Recorded(msg.sender, index, record);
    }

    function completeByIndex(address projectOwner, uint index) public onlyTrainerOfProject(projectOwner, index) {
        _projects[projectOwner].completions[index] = true;

        emit Completed(projectOwner, index);
    }

    function withdrawByIndex(uint index) public payable onlyWithdrawable(index) {
        uint256 withdrawnAmount = _projects[msg.sender].currentAmounts[index];

        _projects[msg.sender].currentAmounts[index] = 0;
        _projects[msg.sender].hasBeenWithdrawn[index] = true;

        _escrow.withdraw(msg.sender);

        emit Withdrawn(msg.sender, _projects[msg.sender].trainers[index], withdrawnAmount);
    }

    function depositOfProjectByIndex(uint index) public view returns(uint256) {
        return _projects[msg.sender].currentAmounts[index];
    }

    function getProjects() public view returns(uint256[] memory, address[] memory) {
        uint256[] memory projectsFlatten = new uint256[] (_projects[msg.sender].count * 3);
        address[] memory projectsTrainerFlatten = new address[] (_projects[msg.sender].count);

        for (uint index = 0; index < _projects[msg.sender].count; index++) {
            projectsFlatten[index * 3]     = _projects[msg.sender].currentAmounts[index];
            projectsFlatten[index * 3 + 1] = _projects[msg.sender].completions[index] ? 1 : 0;
            projectsFlatten[index * 3 + 2] = _projects[msg.sender].hasBeenWithdrawn[index] ? 1 : 0;
            projectsTrainerFlatten[index] = _projects[msg.sender].trainers[index];
        }

        return (projectsFlatten, projectsTrainerFlatten);
    }
}
