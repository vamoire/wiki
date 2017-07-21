# Object-C

## NSString截取
字符串从第n 位开端截取，直到最后 （substringFromIndex:n）（包含第 n 位）
<pre>
    NSString  *a = ＠"i like long dress";
    NSString *b = [a substringFromIndex:4];
    NSLog（＠"\n b: ％＠"，b）;
</pre>