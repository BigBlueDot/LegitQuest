var MessageBox = React.createClass({
    getInitialState: function() {
        return { messages:[] }
    },
    render: function () {
        return (
                <div id="messagebox" className="text-primary message-box">
                    {this.state.messages.map(function(message, i) {
                        return                         <div key={"m" + i}> {message}</div>;
                    })}
                </div>
            );
    },
    addMessage: function (message) {
        this.state.messages.push(message);
    }
});