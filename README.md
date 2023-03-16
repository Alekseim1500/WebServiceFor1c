## Оглавление

- [Описание проекта](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#описание-проекта)

- [Инструкция к работе с C#](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#инструкция-к-работе-с-c)

- [Инструкция к работе с 1С](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#Инструкция-к-работе-с-1С)

- [Инструкция к работе с Apache Kafka](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#инструкция-к-работе-с-apache-kafka)
  - [Установки](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#для-установки-нужно)
  - [Запуск](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#запуск-kafka-и-zookeeper)
  - [Команды для работы с Apache Kafka](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#необходимые-команды-для-работы-с-apache-kafka)
  - [Полезные ссылки для Apache Kafka](https://github.com/Alekseim1500/WebServiceFor1c/blob/main/README.md#источники-для-работы-с-apache-kafka)


# Описание проекта
Данный репозиторий содержит web-сервис написанный на с# для взаимодействия между информационными базами 1с.





# Инструкция к работе с C#
#### Как запустить
Что бы запустить приложение, нужно установить visual studio и обновить пакеты NuDet при необходимости. После этого можно будет редактировать код и запускать его через visual studio. 
Что бы сделать из приложения сайт, нужно опубликовать проект. Для этого нужно выполнить следующие шаги:  
•	В обозревателе решений ПКМ по самому проекту в visual studio -> Опубликовать  
•	В появившейся вкладке нажимаем Создать  
•	Далее на каждой вкладке выбираем следующее: Веб-сервер (IIS) -> Веб-развёртывание -> Заполняем необходимые поля (Сервер: localhost, Имя сайта: Site, URL-адрес назначения: http://localhost:51992/) и нажимаем готов  
•	Заходим в IIS, ПКМ по сайты -> добавить веб-сайт -> заполняем поля (IP адрес это ip на котором разворачивается сайт)
•	Перезапускаем visual studio от имени администратора и нажимаем Опубликовать  

#### Код
Программа начинает работу в файле Global.asax.cs. В методе Application_Star в отдельные потоки запускаются функции: Methods1C8.ResponseAsync получает сообщения от 1с и отправляет их в kafka, Methods1C8.PostObject получает сообщение из kafka и отправляет в 1с.  
Вся информация, которая определяет такие параметры как кто является продюсером, на каком ip расположенна kafka и тому подобное, расположенны в файле My.config.



# Инструкция к работе с 1С
### 1С – что это такое?
1С:Предприятие — программный продукт компании «1С», предназначенный для автоматизации деятельности на предприятии.
«1С:Предприятие» предназначено для автоматизации любого бизнес-процесса предприятия. Наиболее известны продукты по автоматизации бухгалтерского и управленческого учётов (включая начисление зарплаты и управление кадрами), экономической и организационной деятельности предприятия.

### Учебная версия 1С
Так как программа «1С:Предприятие» позиционирует себя как платное приложение, для знакомства и изучения этой программы использовалась учебная версия «1С:Предприятие 8.3, учебная версия (8.3.22.1709)» (Windows). Для установки данной версии необходимо зайти на официальный сайт https://online.1c.ru/catalog/free/learning.php. На данном сайте представлены различные версии приложения «1С:Предприятие». Необходимо в верхнем меню выбрать вкладку 1С после выбрать 1С:Предприятие 8. Учебные версии, на открывшейся странице выбрать подходящую учебную версию. В моем случае это «1С:Предприятие 8.3, учебная версия (8.3.22.1709)» (Windows). После того, как зашли на страницу выбранной версии, нажать на ссылку "Получить продукт бесплатно", на открывшейся странице ввести запрашиваемые данные (ФИО, электронная почта). После чего на электронную почту придет ссылка для скачивания архива.

### Установка учебной версии 1С 
После скачивания файла распаковываем полученный архив, в нем заходим в папку platform. В самом низу папки будет находится файл с расширение .exe для установки приложения. Запускаем установку файла. После установки мы получаем приложение готовое для работы, однако это пустое приложение, то есть именно в нем нет никаких либо конфигураций и всё логику работы мы прописываем самостоятельно. Чтобы избежать лишней работы у программы «1С:Предприятие» есть готовые конфигурации для разных задач. В нашем случае использовалась для ознакомления с готовыми решениями в учебной версии рассматривалась конфигурация бухгалтерия.предприятие. В скаченном архиве данная конфигурация находится в папке accounting. В этой папке также находим установщик и запускаем его. После при создании новой базы у нас будет выбор либо создать пустую базу, либо базу с использованием конфигурации.

### Ознакомление и изучение работы с «1С:Предприятие 8.3, учебная версия (8.3.22.1709)» (Windows) 
Для пошагового изучения «1С:Предприятие»  в большой степени использовался следующий видеокурс на youtube https://www.youtube.com/watch?v=gXYUsQcT7JI&list=PL6Nx1KDcurkBdxssD1k56SDnwuTuX2bBr на данном курсе пошагово объяснялись азы использования программы. а также уроки по написанию кода на языке программирования приложения 1с, написание запросов, а также создание документов. справочников, табличных и печатных форм и работы с ними

### WEB и HTTP сервисы в 1С
Значительным отличием данной версии приложения от 1с 7.7 и от 1с8.2 это наличие WEB и HTTP сервисов соответственно (В версии 1с 7.7 отсутствуют WEB и HTTP сервисы, а в версии 1с8.2 отсутствуют HTTP сервисы. Именно по этим причинам, для работы была выбрана 1с8.3 версия платформы). WEB и HTTP сервисы — две технологии, позволяющие получить доступ к 1С из внешней системы. Причем можно получить доступ как за файрволом, так и через прокси. В общем, практически из любой точки земного шара. С точки зрения 1С, это два объекта метаданных, которые позволяют нам выполнять эти операции. Реализовывается доступ по классической трехзвенной схеме: это СУБД, в качестве сервера выступают кластер серверов 1С и веб-серверы, и клиент, подключающийся к сервису. Изначально сервисы разрабатывались для поддержки внешних систем: сайтов, интернет-магазинов, корпоративных порталов. В дальнейшем технология получила широкое распространение и сейчас используется в широком спектре схожих задач:
•	Обмен с внешними системами (сайты, магазины, мобильные приложения),
•	Обмен данными между базами 1С (гетерогенные, РИБ),
•	Работа с внешним оборудованием (телефония, ТСД, весы),
•	Предоставление упрощенного интерфейса для пользователей,
•	Предоставление API для сторонних систем или партнеров.
Для более подробного ознакомления с этими двумя сервисами можно перейти в следующий интернет ресурс https://infostart.ru/1c/articles/1523650/. Для начального разбора сервисов и попыток создание элементарного общения 1с с сервером использовались следующие интернет ресурсы https://webhamster.ru/mytetrashare/index/mtb207/1473340804tg2mplwm2d и видеоурок на youtube https://www.youtube.com/watch?v=4xju7pRcOdg&t=2579s.

### Проблема учебной версии
Как понятно из названия, используемая нами версия «1С:Предприятие» имеет ряд ограничений в работе. Одним из этих ограничений была невозможность использовать получившуюся базу в многопользовательском режиме. Для решения этой проблемы была установлена ломанная версия 8.3.18.1289.


# Инструкция к работе с Apache Kafka
#### Apache Kafka
**Apache Kafka** — распределённый программный брокер сообщений. Написан на языках программирования Java и Scala.
Спроектирован как распределённая, горизонтально масштабируемая система, обеспечивающая наращивание пропускной способности как при росте числа и нагрузки со стороны источников, так и количества систем-подписчиков. Подписчики могут быть объединены в группы.
___
#### Для установки нужно:
Включить Hyper-V в панели управления -> Программы и компоненты -> добавить-> Диспетчер Hyper-V -> Создать виртуальную машину.
При установке сервера принимаем все требования и устанавливаем SSH (Ссылка на видео установки в источниках).
На виртуальной машине устанавливаем JRE, после чего устанавливаем сам Apache kafka
*curl -LO https://dlcdn.apache.org/kafka/3.2.1/kafka_2.13-3.2.1.tgz*
И распаковываем
*tar -xvzf /tmp/kafka_2.13-3.2.1.tgz --strip 1*

#### Запуск Kafka и zookeeper

Создание сервисов:\
sudo nano /etc/systemd/system/kafka.service  
Вставить:  
[Unit]  
Requires=zookeeper.service  
After=zookeeper.service  
[Service]  
Type=simple  
User=kafka  
ExecStart=/bin/sh -c '/home/kafka/kafka/bin/kafka-server-start.sh /home/kafka/kafka/config/server.properties > /home/kafka/kafka/kafka.log 2>&1'  
ExecStop=/home/kafka/kafka/bin/kafka-server-stop.sh Restart=on-abnormal [Install]  
WantedBy=multi-user.target  

sudo nano /etc/systemd/system/zookeeper.service  

[Unit]   
Requires=network.target remote-fs.target   
After=network.target remote-fs.target   
[Service] Type=simple   
User=kafka   
ExecStart=/home/kafka/kafka/bin/zookeeper-server-start.sh /home/kafka/kafka/config/zookeeper.properties   
ExecStop=/home/kafka/kafka/bin/zookeeper-server-stop.sh Restart=on-abnormal [Install]   
WantedBy=multi-user.target  


**Для запуска используем (при наличии сервисов)**  
*sudo systemctl start/stop/status kafka/zookeeper(start/stop/status)*  
**Или запускаем напрямую через скрипты**  
*~kafka/bin/zookeeper-server-start.sh ~kafka/config/zookeeper.properties* (Запуск zookeeper)  
*~/kafka/bin/kafka-server-start.sh ~/kafka/config/server.properties* (Запуск kafka)  

**Запуск скриптов для остановки**  
*/home/kafka/kafka/bin/kafka-server-stop.sh*  
*/home/kafka/kafka/bin/zookeeper-server-stop.sh*  

**Для обращения к Брокерам используется:**  
*--bootstrap-server localhost:9092 --zookeeper localhost:2181*  
#### Необходимые команды для работы с Apache Kafka  
**Лист топиков:** *~/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --list*  

**Создать топик:** *~/kafka/bin/kafka-topics.sh --create --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic Название_топика*

**Отправление(с созданием топика):** *Что_отправить | ~/kafka/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic Название_топика*

**Удаление топика:** *~/kafka/bin/kafka-topics.sh --delete --topic Название_топика --bootstrap-server localhost:9092*

**Отправка сообщений:** *~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic Навзание_топика*

**Отправка сообщений с ключом:** *~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic Название_топика --property "key.separator=-" --property "parse.key=true"*

**Принятие сообщений:** *~/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic Название_топика --from-beginning*

**Принятие сообщений с ключом:** *~/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic Название_топика --from-beginning --property "key.separator= - " --property "print.key=true"*
(key.separator – разделитель ключ-значение)


**Узнать время хранения сообщений:** *grep -i 'log.retention.[hms]. * \=' config/server.properties*

**Полные настройки топика:** *~/kafka/bin/kafka-configs.sh --describe --all --bootstrap-server=localhost:9092 --topic Название_потика*

**Таймер для отдельного топика:** *~/kafka/bin/kafka-configs.sh --alter --add-config retention.ms = 300000 --bootstrap-server localhost:9092 --topic Название_топика*

**Также присутствуют:** *log.retention.hours,log.retention.minutes и log.retention.ms*

**Подключение к ZooKeeper:** *~/kafka/bin/zookeeper-shell.sh localhost:2181*

**Узнать лист топиков(после подключения ZooKeeper):** *ls /brokers/ids*

**Информация о топиках:** *~/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --describe*

**Узнать занятые порты:** *netstat -pnltu*
**Завершить процесс:** *kill -9 pid*

**Список групп:** *~/kafka/bin/kafka-consumer-groups.sh --bootstrap-server localhost:9092 --list*

**Удалить группу пользователей:** *~/kafka/bin/kafka-consumer-groups.sh --bootstrap-server localhost:9092 --delete --group Название_группы*




**Открытие kafka для подключения:**
*sudo iptables -A INPUT -p tcp --dport 9092 -j ACCEPT*
заменить *advertised.listeners* в *server.properties* на *advertised.listeners=PLAINTEXT://192.168.205.106:9092*




#### Источники для работы с Apache kafka

https://ruvds.com/ru/helpcenter/kak-ustanovit-apache-kafka-na-ubuntu-20-04/ Установка Apache kafka на сервер  
https://www.youtube.com/watch?v=fOh98R9usck&t=702s Видео про несколько серверов  
https://www.tutorialspoint.com/apache_kafka/apache_kafka_basic_operations.htm Создание нескольких брокеров  
https://vk.com/doc565756056_591392461?hash=bNCJxh3bbCLTfpl0RUzs96KlkjsXu2tuw6KyJN11FUw Apache kafka потоковая обработка и анализ данных  
https://russianblogs.com/article/8418667672/ Параметры для Kafka из консоли  
https://www.youtube.com/watch?v=EAphCykdjA0 Установка linux ubuntu server  

