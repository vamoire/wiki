//cocos2dx sqlite3 使用

#include "sqlite3/include/sqlite3.h"

//数据库地址
std::string path = FileUtils::getInstance()->getWritablePath() + "0308.db";
std::string sql;
//打开数据库 || 创建数据库
int ret;
sqlite3* pdb = nullptr;
ret = sqlite3_open(path.c_str(), &pdb);
if (ret != SQLITE_OK) {
    const char* errmsg = sqlite3_errmsg(pdb);
    log("sqlite open error: %s", errmsg);
    sqlite3_close(pdb);
    return;
}
//创建表
sql = "create table student(ID integer primary key autoincrement, name text, sex text)";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK) {
    log("create table fail");
}
//插入数据
sql = "insert into student values(1, 'student1', 'male')";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK){
    log("insert data fail");
}
sql = "insert into student values(2, 'student2', 'male')";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK){
    log("insert data fail");
}
sql = "insert into student values(3, 'student3', 'male')";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK){
    log("insert data fail");
}
//查询数据
sql = "select * from student";
ret = sqlite3_exec(pdb, sql.c_str(), [](void *para, int col_num, char** col_value, char**col_name){
    log("%s一共有%d个字段", (char*)para, col_num);
    for (int i = 0; i < col_num; ++i) {
        log("%s = %s", col_name[i], col_value[i]);
    }
    return 0;
}, (void*)"para", nullptr);
//删除数据
sql = "delete from student where ID = 1";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK) {
    log("delete data fail");
}
//修改数据
sql = "update student set name = 'hello' where ID = 3";
ret = sqlite3_exec(pdb, sql.c_str(), nullptr, nullptr, nullptr);
if (ret != SQLITE_OK) {
    log("update data fail");
}
//查询数据
char** table;
int r, c;
sql = "select * from student";
sqlite3_get_table(pdb, sql.c_str(), &table, &r, &c, nullptr);
log("行数 is %d, 列数 is %d", r, c);
for (int i = 0; i <= r; ++i) {
    for (int j = 0; j < c; ++j) {
        log("%s", table[i * c+ j]);
    }
}
sqlite3_free_table(table);
//关闭数据库
sqlite3_close(pdb);