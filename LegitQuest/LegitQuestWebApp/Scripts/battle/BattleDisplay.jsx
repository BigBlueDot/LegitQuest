var BattleDisplay = React.createClass({
    getInitialState: function () {
        return { messages: [] }
    },
    render: function () {
        return (<div className="battle-container container-fluid">
                    <MessageBox ref="messageBox" key="mb"></MessageBox>
                    <CharacterDisplay key="cd"></CharacterDisplay>
                </div>
            );
    },
    componentDidMount: function () {
        var self = this;

        var clientConnection = $.connection.battleHub;

        clientConnection.client.processMessage = function (message) {
            if (message.message != null && message.message != "") {
                self.refs.messageBox.setState({ messages: self.refs.messageBox.state.messages.concat([message.message]) });
                //messageBoxElement.setState({ messages: this.messages + message });
                //messageBoxElement.getProperties().messages.push(message.message);
                // messageBox.innerHTML = message.message + "<br />" + messageBox.innerHTML;
            }
        }

        $.connection.hub.start().done(function () {
            clientConnection.server.startCombat();
        })
    }
});