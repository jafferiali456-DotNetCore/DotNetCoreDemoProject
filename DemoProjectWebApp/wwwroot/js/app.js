
    function onClose() {
        $("#showDialogBtn").fadeIn();
    }

    function onOpen() {
        $("#showDialogBtn").fadeOut();
    }

    function showDialog() {
        $('#dialog').data("kendoDialog").open();
    }

    function onSkipVersionClick() {
        // Insert any custom JavaSctipt logic when "Skip this version" is selected.
        // For example, call a specified JavaScrip function to execute.
    }


    function error_handler(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }
    function showDialog(id) {
        selectedId = id; // Store the selected ID
        $('#confirmDialog').modal('show'); // Display the modal
    }

    // Handle the "OK" button click in the confirmation dialog
    $('#deleteConfirmButton').on('click', function () {
        // Call your server-side delete action via AJAX
        deleteItem(selectedId);
        $('#confirmDialog').modal('hide'); // Hide the modal after deletion
    });


<script type="text/javascript">
    function onCancel(e) {
        alert("Cancel action was clicked");
    return false; // Returning false will prevent the closing of the Dialog.
    }
</script>



