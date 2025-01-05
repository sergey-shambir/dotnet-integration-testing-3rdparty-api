const express = require('express')
const fs = require('fs')

const app = express()
const port = 5025

const store = function () {
    const dataPath = './data.json'

    /**
     * @typedef {Object} MailSubscriptionData
     * @property {string} name
     * @property {string} email
     * @property {any} customData
     */
    /**
     * @type {Object.<string,MailSubscriptionData[]>}
     */
    const mailSubscriptionsByType = loadData()

    /**
     * @returns {Object.<string,MailSubscriptionData[]>}
     */
    function loadData() {
        if (!fs.existsSync(dataPath))
        {
            return {}
        }
        const data = fs.readFileSync(dataPath).toString()
        return JSON.parse(data)
    }

    /**
     * @param {Object.<string,MailSubscriptionData[]>} data
     */
    function saveData(data) {
        fs.writeFileSync(dataPath, JSON.stringify(data), 'utf8')
    }

    return {
        /**
         * @param {string} type
         * @param {MailSubscriptionData} mailSubscription
         */
        addMailSubscription: function(type, mailSubscription) {
            mailSubscriptions = mailSubscriptionsByType[type] || []
            const index = mailSubscriptions.findIndex((data) => data.name == mailSubscription.name)
            if (index != -1) {
                mailSubscriptions[index] = mailSubscription
            } else {
                mailSubscriptions.push(mailSubscription)
            }
            mailSubscriptionsByType[type] = mailSubscriptions
            saveData(mailSubscriptionsByType)
        },
        /**
         * @param {string} type
         * @returns {MailSubscriptionData[]}
         */
        listMailSubscriptions: function (type) {
            return mailSubscriptionsByType[type] || []
        }
    }
}()

app.use(express.json())

app.post('/mail-subscription/', (req, res) => {
    const type = String(req.body.type)
    const mailSubscription = {
        name: req.body.name,
        email: req.body.email,
        customData: req.body.customData,
    }

    store.addMailSubscription(type, mailSubscription)
    res.end()
})

app.get('/mail-subscription/', (req, res) => {
    const type = String(req.query.type)
    const mailSubscriptions = store.listMailSubscriptions(type)

    res.setHeader('Content-Type', 'application/json')
    res.end(JSON.stringify(mailSubscriptions))
})

app.listen(port, () => {
    console.log(`MailSubscription service listening on port ${port}`)
})