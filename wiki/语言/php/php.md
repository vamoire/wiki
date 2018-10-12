# PHP

## ------------ CakePhp ------------

## 身份验证
- beforeFilter
```
    public function beforeFilter()
    {
        parent::beforeFilter();

        $ok = false;
        do {
            //用户信息
            $ret = $this->Session->check('User.Info');
            if (!$ret) break;

            $UserInfo = $this->Session->read('User.Info');
            if (empty($UserInfo)) break;

            //商户ID
            $companyID = $UserInfo['companyID'];
            if (empty($companyID)) break;

            $ok = true;
        }while(false);

        if (!$ok) {
            //异常处理
            $this->redirect(array('controller'=>'Login','action'=>'login'));
        }
    }
```

## Model 数据库操作
- 查询
```
$data = $this->find('first', array(
    'conditions' => array('name' => $userName, 'pass' => $usePass)
));
```

- 新增
```
// 建新记录：id 没有设置或为 null
$this->Recipe->create();
$this->Recipe->save($this->request->data);
//在循环中调用 save 方法时，不要忘记调用 clear()。
```

- 删除
```
delete(int $id = null, boolean $cascade = true);
```

- 更新
```
$this->id = $id;
$data['UserModel']['last'] = date("Y-m-d h:i:sa");
$this->save($data);
//在循环中调用 save 方法时，不要忘记调用 clear()。
```

## Session
- 写入
```
this->Session->write('User.Info', $data);
```

- 删除
```
$this->Session->delete('User.Info');
$this->Session->destroy();
```

- 其他
```
$this->Session->read('User.Info');
$this->Session->consume('User.Info');
$this->Session->check('User.Info');
$this->Session->setFlash('data');
$this->Session->flash();
```

## 跳转
- php
```
$this->redirect(array('controller'=>'Logins','action'=>'login'));
```

- button js
```
onclick="location = '<?php echo $this->base;?>/Manager/createRecruitment'"
```

## 占位符
- default.ctp
```
<?php echo $this->fetch('title');?>
```

- view.ctp
```
<?php echo $this->assign('title','登录')?>
```

## 文件访问目录
- View
```
<?php echo $this->base;?>/Logins/login
```

- webroot
```
<?php echo $this->webroot;?>/img/icon.jpg
```

## Controller和Model交互
- Controller引用Model
```
public $uses = array('Bbb', 'xxx', 'ttt');
```

- Controller调用Model
```
public function test($a, $b) {
    $this->Bbb->cccc($a, $b);
    $this->xxx->cccc($a, $b);
    $this->ttt->cccc($a, $b);
    $this->Bbb->find();
}
```

## Controller和View交互
- Controller设置View数据
```
public function setData() {
    $this->set('k1','k2');
    $this->set(array('k1', 'k2'),array('v1', 'v2'));
}
```

- Controller获取View数据
```
public function getData() {
    print_r($_POST);
    $this->autoRender = false;
}
```

- View显示Controller数据
```
<?php echo $k1?>
```

- View提交表单到Controller
```
<form method="post" action="<?php echo Router::url(array('controller'=>'Tests','action'=>'bbb'));?>">
    <input name="ttt">
    <button type="submit" value="rrr">按钮</button>
</form>
```

## 不创建View
```
public function test() {
    $this->autoRender = false;
}
```

## ------------ PHP ------------

## 文件上传

- html

```
<form id="edit-form" style="margin-left: 20px;margin-top: 20px" method="post"
      enctype="multipart/form-data">
    <input type="file" name="goods_icon_file" id="goods_icon_file" />
</form>
```

- script

```
var goods_icon_file = $('#goods_icon_file').val();

var edit_form = document.getElementById('edit-form');
var formData = new FormData(edit_form);
formData.append('goods_icon_file', goods_icon_file);

$.ajax({
    type: "post",
    url: '<?php echo $this->base;?>/Manager/tryEditPreferentialGoods',
    data: formData,
    dataType: 'json',
    cache:false,
    processData:false,
    contentType:false,
    success: function (data) {
        console.log(data);
    },
    error: function (msg) {
        console.log(msg);
    }
});
```

- php

```
$file = $_FILES["goods_icon_file"]
$file_type = $file["type"];
$file_size = $file["size"];
$file_error = $file["error"];
$file_name = $file["name"];
$file_tmp_name = $file["tmp_name"];
if ((($file_type == "image/gif") || ($file_type == "image/jpeg") || ($file_type == "image/pjpeg")) && ($file_size < 20000))
{
    if ($file_error > 0) {
        $file_name = '';
    }
    else {
        //存储路径
        $path = $this->getImageSavePath() . $file_name;
        if (file_exists($path))
        {
        }
        else
        {
            move_uploaded_file($file_tmp_name, $path);
        }
    }
}
else {
    $file_name = '';
}
//图片展示URL
$imageSrc = $this->getImageUrl() . $file_name
```

- [参考1](http://www.w3school.com.cn/php/php_file_upload.asp)

- [参考2](https://segmentfault.com/q/1010000010201732/a-1020000010202186)

- [参考3](https://www.cnblogs.com/labnizejuly/p/5588444.html)

- [参考4](https://blog.csdn.net/ccccc_jun/article/details/79136088)


## 测试
[hello.php](code/hello.php)

## MAC开启PHP
1. 开启apache服务
2. 修改/etc/apache2/httpd.conf，取消下面代码的注释
<pre>
LoadModule php5_module libexec/apache2/libphp5.so
</pre>
3. 重启apache服务
<pre>
sudo apachectl restart
</pre>
4. 相关链接
[PHP教程][1]
[MAC下的PHP环境搭建][2]

[1]:http://www.w3school.com.cn/php/php_install.asp
[2]:http://blog.csdn.net/wj_november/article/details/51417491