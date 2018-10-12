# Web开发

## 获取元素属性
```
var logo = $('#logo').attr("src");
```

## 点击元素关闭
```
<script>
    document.addEventListener("click", function(e){
        if (e.target === $('#jionUs')[0]) {
            $('#jionUs')[0].style.display = "none";
        }
    });
</script>
```

## AJAX

- [ajax与php交互](http://www.php.cn/js-tutorial-382739.html)

- html
```
<script>
    $(function () {
        $('#submit').click(function () {
            // $('#loginForm').serialize()
            var loginUserName = $('#loginUserName').val();
            var loginUserPass = $('#loginUserPass').val();
            $.ajax({
                type:"post",
                url:'tryLogin',
                data:{loginUserName:loginUserName,loginUserPass:loginUserPass},
                dataType:'json',
                success:function (data) {
                    data = JSON.parse(data);
                    if (data.OK == 'True') {
                        location = '<?php echo $this->base;?>/Statistics/userStatistics';
                    }
                    else {
                        //TODO:提示登录失败
                        console.log(data);
                    }
                },
                error:function (msg) {
                    console.log(msg);
                }
            });
        });
    });
</script>   
```

- php
```
// 登录验证
public function tryLogin() {
    // 不适用自动渲染
    $this->autoRender = false;
    $json = '{"OK":"False"}';
    if ($this->request->is('ajax')) {
        $name = $_POST['loginUserName'];
        $pass = $_POST['loginUserPass'];
        $data = $this->UserModel->userLogin($name, $pass);
        if (!empty($data)) {
            $json = '{"OK":"True"}';
        }
    }
    echo json_encode($json);
}
```

- 文件上传

```
<div>
    <input type="file" name="FileUpload" id="FileUpload">
    <a class="layui-btn layui-btn-mini" id="btn_uploadimg">上传图片</a>
</div>

<script type="text/jscript">
    $(function () {
        $("#btn_uploadimg").click(function () {
            var fileObj = document.getElementById("FileUpload").files[0]; // js 获取文件对象
            if (typeof (fileObj) == "undefined" || fileObj.size <= 0) {
                alert("请选择图片");
                return;
            }
            var formFile = new FormData();
            formFile.append("action", "UploadVMKImagePath");  
            formFile.append("file", fileObj); //加入文件对象

            //第一种  XMLHttpRequest 对象
            //var xhr = new XMLHttpRequest();
            //xhr.open("post", "/Admin/Ajax/VMKHandler.ashx", true);
            //xhr.onload = function () {
            //    alert("上传完成!");
            //};
            //xhr.send(formFile);

            //第二种 ajax 提交

            var data = formFile;
            $.ajax({
                url: "/Admin/Ajax/VMKHandler.ashx",
                data: data,
                type: "Post",
                dataType: "json",
                cache: false,//上传文件无需缓存
                processData: false,//用于对data参数进行序列化处理 这里必须false
                contentType: false, //必须
                success: function (result) {
                    alert("上传完成!");
                },
            })
        })
    })
</script>

```

## JS

## 显示隐藏元素
- 隐藏后保留占用的空间
```
//隐藏
document.getElementById('jionUs').style.visibility = 'hidden';
//显示
document.getElementById('jionUs').style.visibility = 'visible';
```

- 隐藏后释放占用的空间
```
// 显示
document.getElementById('jionUs').style.display = '';
// 隐藏
document.getElementById('jionUs').style.display = 'none';
```

## 重定向
- 
```
window.location = "http://www.baidu.com";
```

- 
```
document.location = "http://www.baidu.com";
```

## 延迟执行
```
setTimeout("autoFillIFrameHeight()", 310)
```

## bootstrap

## 下拉菜单
```
<span class="dropdown">
    <button style="width: 160px" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        面议
        <span class="caret" style="margin-left: 5px"></span>
    </button>
    <ul class="dropdown-menu dropdown-menu-left">
        <li>
            <a href="#">面议</a>
        </li>
    </ul>
</span>
```