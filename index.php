<?php
$iipp = $_SERVER["REMOTE_ADDR"];
if($iipp == "::1") {
    header("Location: index.html");
}
else {
    header("HTTP/1.1 404 Not Found");
}
?>