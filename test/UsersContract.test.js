/* Allow to check if the values match with the values that we provide */
const assert = require('assert');

const AssertionError = require('assert').AssertionError;

const Web3 = require('web3');

/* Here we specify the url o route with which we want to comunicate */
const provider = new Web3.providers.HttpProvider('HTTP://127.0.0.1:7545');

const web3 = new Web3(provider);

const result = require('../scripts/compile');

const { abi, evm } = result.contracts['UsersContract.sol'].UsersContract;

let accounts;
let usersContract;

beforeEach(async () => {
    accounts = await web3.eth.getAccounts();
    usersContract = await new web3.eth.Contract(JSON.parse(JSON.stringify(abi)))
        .deploy({ data: evm.bytecode.object })
        .send({ from: accounts[0], gas: '1000000' })
});

describe('The UsersContracts', async () => {
    it('should deploy', () => {
        assert.ok(usersContract.options.address);
    })

    it('should join a user', async () => {
        const name = "Juan Ignacio";
        const surname = "Benito"
        await usersContract.methods
            .join(name, surname)
            .send({ from: accounts[0], gas: '5000000' })
    })

    it('should retrieve an user', async () => {
        const name = "Juan Ignacio";
        const surname = "Benito"
        await usersContract.methods
            .join(name, surname)
            .send({ from: accounts[0], gas: '5000000' })

        const user = await usersContract.methods.getUser(accounts[0]).call();

        assert.equal(name, user[0]);
        assert.equal(surname, user[1]);
    })

    it('should not allow joining an account twice', async () => {
        await usersContract.methods
            .join("Pedro", "Lopez")
            .send({ from: accounts[1], gas: '5000000' })
        try {
            await usersContract.methods
                .join("Ana", "Diez")
                .send({ from: accounts[1], gas: '5000000' });
            assert.fail('same account can not join twice');
        } catch (error) {
            if (error instanceof AssertionError) {
                assert.fail(error.message);
            }
        }
    })

    it('should throw an error when trying to retrieve a user that is not registered', async () => {
        const name = "Not registered";
        const surname = "Fake";
        try {
            const user = await usersContract.methods.getUser(accounts[0]).call();
            assert.equal(name, user[0]);
            assert.equal(surname, user[1]);
        } catch (error) {
            if (error instanceof AssertionError) {
                assert.fail(error.message);
            }
        }
    })
})