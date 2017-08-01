cc.Class({
    extends: cc.Component,

    properties: {
        server: '',
        message: {
            default: null,
            type: cc.EditBox
        },
    },

    onLoad: function () {
        this._wsiSendBinary = null;
        this.linkServer();
    },

    onDisable: function() {
        if (this._wsiSendBinary) {
            this._wsiSendBinary.close();
        }
    },

    linkServer: function(event, customeventdata) {
        if (this._wsiSendBinary) {
            return;
        }
        this._wsiSendBinary = new WebSocket("ws://" + this.server);
        this._wsiSendBinary.onopen = function(evt) {
            cc.log(evt);
        };
        this._wsiSendBinary.onmessage = function(evt) {
            cc.log(evt);
        };        
        this._wsiSendBinary.onerror = function(evt) {
            cc.log(evt);};
        this._wsiSendBinary.onclose = function(evt) {
            cc.log(evt);
            self._wsiSendBinary = null;
        };
    },

    sendMessage: function(event, customeventdata) {
        if (!this._wsiSendBinary) {
            return;
        }
        if (this.message.string === "") {
            cc.log("message is null");
            return;
        }
        if (this._wsiSendBinary.readyState === WebSocket.OPEN) {
            this._wsiSendBinary.send(this.message.string);
        }
        else {
            cc.log("wait");
        }
    }

});
