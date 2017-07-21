<?php
$host_name = exec("hostname");
$host_ip = gethostbyname($host_name);
if(empty($host_ip) == false && $host_ip == "xiajiajia.local") {
        header("Location: https://www.baidu.com");
}
exit;
?>