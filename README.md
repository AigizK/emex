—татистика запросов пользователей

¬ MasterPage нужно разместить следующий скрипт

<!--EmexInternet counter-->
<script type="text/javascript">
<!--
document.write("<img src='http://emex-stat.ru/stat?l<%= UserID =>;r" +
escape(document.referrer)+";u"+escape(document.URL)+
";"+Math.random()+
"' "+"border='0' width='1' height='1'>")
//-->
</script>
<!--/EmexInternet counter-->

ѕри этом следующий участок кода
<%= UserID =>
надо заменить на логин пользовател€. ј если пользователь не авторизован, то вернуть пустую строчку.