function doRecruit(resources) {
  var urlToShare = "https://tomoto.github.io/xyzgress/";
  var message = resources["message"];
  var dialog = document.createElement("div");
  dialog.title = resources["title"];
  dialog.innerHTML = resources["confirmation"]

  $(dialog).dialog({
    buttons: [
      {
        text: "Google+",
        click: function() {
          document.location =
            "https://plus.google.com/share?url=" + encodeURIComponent(urlToShare) + "&prefilltext=" + encodeURIComponent(message);
          $(this).dialog("close");
        }
      },
      {
        text: "Twitter",
        click: function() {
          document.location =
            "http://twitter.com/share?url=" + encodeURIComponent(urlToShare) + "&text=" + encodeURIComponent(message);
          $(this).dialog("close");
        }
      },
      {
        text: resources["cancel"],
        click: function() {
          $(this).dialog("close");
        }
      },
    ],
    position: { of: "#gameplayer" },
    modal: true
  });
}
