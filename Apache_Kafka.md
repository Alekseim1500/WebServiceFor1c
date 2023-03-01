### Apache Kafka
**Apache Kafka** — распределённый программный брокер сообщений. Написан на языках программирования Java и Scala.
Спроектирован как распределённая, горизонтально масштабируемая система, обеспечивающая наращивание пропускной способности как при росте числа и нагрузки со стороны источников, так и количества систем-подписчиков. Подписчики могут быть объединены в группы.
___
# Для установки нужно:
Включить Hyper-V в панели управления -> Программы и компоненты -> добавить-> Диспетчер Hyper-V -> Создать виртуальную машину.
При установке сервера принимаем все требования и устанавливаем SSH (Ссылка на видео установки в источниках).
На виртуальной машине устанавливаем JRE, после чего устанавливаем сам Apache kafka
*curl -LO https://dlcdn.apache.org/kafka/3.2.1/kafka_2.13-3.2.1.tgz*
И распаковываем
*tar -xvzf /tmp/kafka_2.13-3.2.1.tgz --strip 1*

Создание сервисов:
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
## Необходимые команды для работы с Apache Kafka
**Лист топиков:** *~/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --list*

**Создать топик:** *~/kafka/bin/kafka-topics.sh --create --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic Название_топика*

**Отправление(с созданием топика):** *Что_отправить | ~/kafka/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic Название_топика*

**Удаление топика:** *~/kafka/bin/kafka-topics.sh --delete --topic Название_топика --bootstrap-server localhost:9092*

**Отправка сообщений:** *~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic Навзание_топика*

**Отправка сообщений с ключом:** *~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic Название_топика --property "key.separator=-" --property "parse.key=true"*

**Принятие сообщений:** *~/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic Название_топика --from-beginning*

**Принятие сообщений с ключом:** *~/kafka/bin/kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic Название_топика --from-beginning -property "key.separator= - " --property "print.key=true"*
(key.separator – разделитель ключ-значение)


**Узнать время хранения сообщений:** *grep -i 'log.retention.[hms]. * \=' config/server.properties*

**Полные настройки топика:** *~/kafka/bin/kafka-configs.sh --describe --all --bootstrap-server=localhost:9092 --topic Название_потика*

**Таймер для отдельного топика:** *~/kafka/bin/kafka-configs.sh --alter --add-config retention.ms = 300000 –bootstrap-server localhost:9092 --topic Название_топика*

**Также присутствуют:** *log.retention.hours,log.retention.minutes и log.retention.ms*

**Подключение к ZooKeeper:** *~/kafka/bin/zookeeper-shell.sh localhost:2181*

**Узнать лист топиков(после подключения ZooKeeper):** *ls /brokers/ids*

**Информация о топиках:** *~/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --describe*

**Узнать занятые порты:** *netstat -pnltu*
**Завершить процесс:** *kill -9 pid*

**Список групп:** *~/kafka/bin/kafka-consumer-groups.sh --bootstrap-server localhost:9092 --list*

**Удалить группу пользователей:** *~/kafka/bin/kafka-consumer-groups.sh --bootstrap-server localhost:9092 --delete –group Название_группы*




**Открытие kafka для подключения:**
*sudo iptables -A INPUT -p tcp --dport 9092 -j ACCEPT*
заменить *advertised.listeners* в *server.properties* на *advertised.listeners=PLAINTEXT://192.168.205.106:9092*




### Источники

https://ruvds.com/ru/helpcenter/kak-ustanovit-apache-kafka-na-ubuntu-20-04/ -- установка Apache kafka на сервер
https://www.youtube.com/watch?v=fOh98R9usck&t=702s – Видео про несколько серверов
https://www.tutorialspoint.com/apache_kafka/apache_kafka_basic_operations.htm Создание нескольких брокеров.
https://vk.com/doc565756056_591392461?hash=bNCJxh3bbCLTfpl0RUzs96KlkjsXu2tuw6KyJN11FUw Apache kafka потоковая обработка и анализ данных
https://russianblogs.com/article/8418667672/ Параметры для Kafka из консоли
https://www.youtube.com/watch?v=EAphCykdjA0 – Установка linux ubuntu server

