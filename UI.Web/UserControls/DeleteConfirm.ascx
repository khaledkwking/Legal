<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteConfirm.ascx.cs" Inherits="UI.Web.UserControls.DeleteConfirm" %>

<style>
    .delete-confirm-overlay {
        display: none;
        position: fixed;
        z-index: 99998;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,.5);
    }

    .delete-confirm-box {
        display: none;
        position: fixed !important;
        z-index: 99999 !important;

        top: 50% !important;
        left: 50% !important;

        transform: translate(-50%, -50%) !important;

        width: 400px;
        max-width: 90%;
        padding: 30px;
        background: #fff;
        border-radius: 10px;
        box-shadow: 0 5px 30px rgba(0,0,0,.4);
        text-align: center;
    }

    .delete-confirm-icon {
        font-size: 45px;
        color: #dc3545;
        margin-bottom: 15px;
    }

    .delete-confirm-message {
        font-size: 18px;
        margin-bottom: 25px;
    }

    .delete-confirm-box button {
        min-width: 100px;
        margin: 5px;
    }
</style>

<div id="deleteConfirmOverlay"
     class="delete-confirm-overlay"
     onclick="DeleteConfirm.cancel();">
</div>

<div id="deleteConfirmBox"
     class="delete-confirm-box">

    <div class="delete-confirm-icon">
        <i class="fa fa-trash"></i>
    </div>

    <div class="delete-confirm-message">
        <%= GetGlobalResourceObject("Alerts", "DeleteAlert") %>
    </div>

    <button type="button"
            class="btn btn-danger"
            onclick="DeleteConfirm.confirm();">
        Delete
    </button>

    <button type="button"
            class="btn btn-secondary"
            onclick="DeleteConfirm.cancel();">
        Cancel
    </button>

</div>

<script>
    var DeleteConfirm = (function () {

        var button = null;

        function show(sourceButton) {

            button = sourceButton;

            document.getElementById("deleteConfirmOverlay").style.display = "block";
            document.getElementById("deleteConfirmBox").style.display = "block";

            return false;
        }

        function cancel() {

            document.getElementById("deleteConfirmOverlay").style.display = "none";
            document.getElementById("deleteConfirmBox").style.display = "none";

            button = null;
        }

        function confirm() {

            if (button) {

                var target = button;

                cancel();

                target.onclick = null;
                target.click();
            }
        }

        return {
            show: show,
            cancel: cancel,
            confirm: confirm
        };

    })();
</script>