namespace Butterfly.system
{
    public struct Ex
    {
        public const string Null = "";

        public struct MainObject
        {
            public const string x10001 = "Вы создали обьект {0} который не находиться в пространсве имен " + GlobalData.SYSTEM_NAMESPACE + ".";
            public const string x10002 = "Обьект должен иметь Type.Name либо NameObject, либо NameObject'3.";
            public const string x10003 = "Вы вызвали Exception c сообщением: \n    {1} \nи передали недостаточное количесво информации для того что бы можно правильно фарматировать строку.";
            public const string x10004 = "Вы не можете вызвать метод {0} потому что обьект {1} является null.";
            public const string x10005 = "Метод {0} можно вызвать только один раз когда обьект находится в состоянии {1}.";
            public const string x10006 = "Ошибка при создании зависимостей в методе {0}.";
            public const string x10007 = "Вы должны добовлять с помощью метода {0} зависимости в стандартном конструкторе класса.";
            public const string x10008 = "Метод {0} вызывается когда состояние обьекта {1}. В данный момент состояние {2}.";
            public const string x10009 = "При вызове метода Construction() произошла ошибка времени сборки.";
            public const string x10010 = "Вы пытаетесь добавить обьект {0} в методе AddObject() который не является типом Controller.";
            public const string x10011 = "Добавить дочерний Controller обьект {0} можно только в методе start() или во время выполнения программы.";
            public const string x10012 = "Добавленый Handler {0} не унаследован от абстрактного класса MainObject.";
            public const string x10013 = "Добавленый обьект {0} не является Handler обьектом.";
            public const string x10014 = "Вы пытаетесь добавить Handler {0} по ключу который уже используется.";
            public const string x10015 = "Создовать Handler обьекты можно только в методе contruction().";
            public const string x10016 = "Не удалось добавить Handler обьект.";
            public const string x10017 = "Не удалось добавить приватный Handler {0}.";
            public const string x10018 = "Вы создаете {0} обработчик в нутри самого себя.";
            public const string x10019 = "Уже создано глобальное прослушивание сообщений с именем {0} в {1}.";
            public const string x10020 = "Никто не прослушивает сообщения {0}.";
            public const string x10021 = "В обьекте {0} не прослушиваются сообщения {1}.";
            public const string x10022 = "Вы передали в создаваемый ControllerObject:<{0}>обьект который должен представлять его локальное значение, " +
                                         "но данный ControllerObject не имеют такого поля с таким типом. Если вам необходимо локальное значение," +
                                         "то укажите это явно. public class {1} : Controller.LocalValue<{0}>";
            public const string x10023 = "Вы передали в обьект {0} локальное значение {1}, но создоваемый обьект не релизует данные значение.";
            public const string x10024 = "Вы передали в INode.Define параметр System.Action который служит лишь для системных операций. Для остальных видов обьектов этот параметр использовать нельзя!!!";
            public const string x10025 = "Создоваемый Controller обьект {0} с именем {1} уже сущесвует.";
            public const string x10026 = "!!!!!! Вы неможете вызывать метод destroу() в Contruction() !!!!!!! destroy() предназначем для вызова в методе Configurat() или во время жизни.";
            public const string x10027 = "!!!!!! Вы неможете вызывать метод destroу() в Start() !!!!!!! destroy() предназначем для вызова в методе Configurat() или во время жизни.";
            public const string x10028 = "Вы указали невыреный input параметр (ref {0} rInput) для обработчика";
            public const string x10029 = "";
            public const string x10030 = "";
            public const string x10031 = "";
            public const string x10032 = "";
            public const string x10033 = "";
            public const string x10034 = "";
            public const string x10035 = "";
            public const string x10036 = "";
            public const string x10037 = "";
            public const string x10038 = "";
            public const string x10039 = "";
        }

        public struct MainObjectsManager
        {
            public const string x10001 = "Вы не определили все возможные состояния которые нужно запустить в момент создания ветки для дачерних обьектов.";
            public const string x10002 = "Если в качесве имени вы указываете создоваемому обьекту {0} число, то оно не должно превышать значение {}(uint.MaxValue).";
            public const string x10003 = "Обьект {0} ожидает локальное значение типа {1}, передайте его в параметр в одном из вызовов.";
            public const string x10004 = "Вы пытаетесь повторно задать локальное значение обьекту {0}.";
            public const string x10005 = "0} можно создать только в методе Construction() или во время жизни.";
            public const string x10006 = "Вы пытаетесь получить по ключу {0} обьект типа {1}, но под данным ключом хранится обьект типа {2}.";
            public const string x10007 = "Вы пытаетесь удалить обьект {0} по ключу {1}, но такого обьекта не сущесвует.";
            public const string x10008 = "Вы пытаетесь создать обьект типа {0}, в нутри самого себя. Произайдет бесконечное зацыкливание.";
            public const string x10009 = "Вы вызвали метод IIndependent.Stop() у обьекта который не является таковым.";
            public const string x10010 = "Вы не можете создовать в методе Start() обьекты с помощью метода create_object(), воспользуйтесь методом add_object().";
            public const string x10011 = "Вы создали NodeMain обьект {0} с помощью отложеного вызова. Теперь этот обьект является частью своего родителя и он не может быть Independent.";
            public const string x10012 = "Создать отложеный вызов Controller обьекта {0} можно только в методе Start().";
            public const string x10013 = "Создать Controller обьект {0} можно только после вызова метода Start(), тоесть во время активной жизни текущего обьекта.";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct GlobalObjectsManager
        {
            public const string x10001 = "Создать GlobalEcho<{0}> можно только в методе Construction().";
            public const string x10002 = "Вы пытаетесь создать/воспользоваться прослушиванием сообщений {0} в обьекте который находится в состоянии отличном от {1}. " +
                "Любое взаимодейсвие с глобальными обьетами происходит в методе Construction().";
            public const string x10003 = "Вы пытаетесь получить по ключю {0} глобальный обьект {1} который реализует прослушивание входящих данных," +
                                "но под этим ключом уже создан другой глобальный обьект в {2}. Укажите другой ключ.";
            public const string x10004 = "Вы указали 2 GlobalEcho прослушивающих одинаковый тип данных {0}, но одному указали имя не явно, а второму явно. Укажите обоим echo имена явно.";
            public const string x10005 = "Вы создали два одинаковых g_echo<{0}> в одной директории. Один либо лишний и удалите его, либо явно задайте им имена.";
            public const string x10006 = "Вы создали два GlobalEcho<{0}> c одинаковыми именами в одной директории.";
            public const string x10007 = "Ты пытаешся задать родительские глобальные обьекты через метод set(), но они уже заданы. " +
                "Скорее всего ты чото накасячил где то в MainObject. Родительские обьекты назначаются при создании ветки " +
                "и между вызовами Construction() в родительском обьекте и конструкторами в его детях.";
            public const string x10008 = "Глобальный echo обьект c именем {0} уже был создан в директории {1}.";
            public const string x10009 = "Вы не задали интерфейс {0} в обьекте {1}.";
            public const string x10010 = "Создать глобальную рассылку с помощью метода sending_message<{0}> можно только в методе Contruction().";
            public const string x10011 = "Вы пытаетесь создать глобальную рассылку с помощью метода sending_message<{0}> с именем {1}, но такой обьект уже создан в директории {2}.";
            public const string x10012 = "Вы создали глобальную рассылку {0}, но этот обьект не реализует интерфейя {1}.";
            public const string x10013 = "Создать прослушиваение из {0} рассылки сообщений типа {1} можно только в методе Contruction().";
            public const string x10014 = "Обьекта рассылающего сообщение с именем {0}, не сущесвует.";
            public const string x10015 = "Вы пытаетесь получить рассылку из обьекта {0} который не является вашим родителем. Он находится в {1}.";
            public const string x10016 = "Вы пытаетесь подписаться на рассылку сообщений типа {0}, но обьект с именем {1} рассылает сообщения другого типа.";
            public const string x10017 = "Вы пытаетесь удалить глобальный обьект {0} созданый по ключу {1} из текущего обьекта, хотя он был создан в {2}.";
            public const string x10018 = "Вы создали глобальный обьект {0} с ключом {1} не реализоваа в нутри интерфейс {2}.";
            public const string x10019 = "Вы можете создать глобальный {0}, только в методе Contruction().";
            public const string x10020 = "Вы можете создать тунель для отправки сообщений типа {0} в {1} только в методе Contruction().";
            public const string x10021 = "Вы пытаетесь создать тунель для отправки сообщений типа {0} в {1}, но слушателя с таким именем нету.";
            public const string x10022 = "Вы пытатесь создать тунель с обьектом {0} который не является вашим родителем. Данный обьект находится в директории {1}.";
            public const string x10023 = "Вы пытаетесь создать тенуль к слушателю {0}, на такого слушателя нету.";
            public const string x10024 = "Вы пытаетесь создать тунель к слушателю {0} для отправки сообщений типа {1}, но слушатель с таким именем принимает сообщения типа{2}.";
            public const string x10025 = "Неудалось найти родительский обьект {0} который бы прослушавал ваше сообщение типа {1}.";
            public const string x10026 = "Вы не реализовали интерфейс в {0}.";
            public const string x10027 = "";
            public const string x10028 = "";
            public const string x10029 = "";
        }

        public struct LocalObject
        {
            public const string x10001 = "Воспользоваться локальным обьектом {0} можно только в методе construction() и start().";
            public const string x10002 = "Попытка создания двух разных локальных обьектов {0} без явного указания их имени.";
            public const string x10003 = "Вы не определили все возможные состояния которые нужно запустить в момент создания ветки для публичные обработчиков.";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct Thread
        {
            public const string x10001 = "Вызвать фуркций add_thread() можно только в методе Start().";
            public const string x10002 = "Вы пытаетесь запустить патоки находять в жиненом цикле {0}, а должны находится в {1}.";
            public const string x10003 = "Поток {0} завершается больше {1}. Слишком долго.";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct Handler
        {
            public const string x10001 = "Не соответсвие входного типа в методе {0}. Выходной тип {1}, а вы пытаетесь принять {2}.";
            public const string x10002 = "Не соответсвие типов. Вы приняли данные типа {0}, а ожидался тип {1}.";
            public const string x10003 = "Вы пытаетесь создать обработчик {0} в нутри самого себя.";
            public const string x10004 = "Метод Remove() вызывается только в момен остановки Node обьекта в котором находится данный обработчик. Ожидается что его состяние будет {0}, но оно {1}.";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct ActionValue
        {
            public const string x10001 = "Устанавливать связь для обьекта {0} можно только в методе Construction().";
            public const string x10002 = "Не соответсвие типов. Неудалось перенаправить {0} в {1}.";
            public const string x10003 = "Установить async_input метод можно только в методе Construction().";
            public const string x10004 = "При создании {0} ты не реализовал интерфейс {1}.";
            public const string x10005 = "Невозможно задать такой тип работы обьекта с echo {0}. Он должен быть либо {1}, либо {2}.";
            public const string x10006 = "Вы пытаетесь установить связь с глобальным echo.Но слушателя echo с именем {0} не сущесвует.";
            public const string x10007 = "Невозможно утановить связь для {0}.Устанавливать прямые связи можно только в методе contruction().";
            public const string x10008 = "Ошибка соответсвия типов. Неудалось перенаправить данные {0} в {1}";
            public const string x10009 = "Неудалось создать приватный обработчик типа {0}.";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct Func
        {
            public const string x10001 = "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct EchoReturn
        {
            public const string x10001 = "Вы не можете установить связь с прослушивающим echo, так как ожидаемый тип принимаемых данных {0}, а вы отправили данные типа {1}.";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct PollClient
        {
            public const string x10001 = "Вы пытаетесь подписатся/создать пулл, но обьект Explorer MainObject'a пустой.";
            public const string x10002 = "Вы пытаетесь подписатся/создать пулл, но подписываемое событие равно null.";
            public const string x10003 = "Вы пытаетесь подписатся/создать пулл, но тип создоваемого пула равен Poll.None.";
            public const string x10004 = "Вы пытаетесь подписатся/создать пулл, но размер но вы не указали максимальный размер пула.";
            public const string x10005 = "Вы пытаетесь подписатся/создать пулл, но временой интервал равен 0.";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct LocalValue
        {
            public const string x10001 = "Вы уже задали локальное значение.";
            public const string x10002 = "Вы передали локальное значение неверного типа {0}, ожидалось {1}.";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct SubscibtionManager
        {
            public const string x10001 = "Добавление методов на подписку и отписку должно происходить когда обьект находится в состоянии {0}, " +
                "но в данный момент обьект находится в состоянии {1}.";
            public const string x10002 = "Вы уже добавили данный вид подписки {0}, не нужно делать это повторно.";
            public const string x10003 = "Добавление обьекта на отподписку должно происходить когда обьект находится в состоянии {0}, " +
                "но в данный момент обьект находится в состоянии {1}.";
            public const string x10004 = "Вы уже добавили данный вид отподписки {0}, не нужно делать это повторно.";
            public const string x10005 = "Вы получили лишнее сообщение об окончании подписки.";
            public const string x10006 = "Вы получили лишнее сообщение об окончании отписки.";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct SendingMessage
        {
            public const string x10001 = "Вы пытаетесь подписать обьект {0} на рассылку сообщений, но этот обьект не реализует интерейс {1}.";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct SubscribeObjectsGlobal
        {
            public const string x10001 = "Вы пытаетесь повторно подписаться на рассылку сообщений в {0} из одного Controller обькта," +
                " проверте созданые Handler обьекты возможно вы продублировали создание в них.";
            public const string x10002 = "Вы получили лишние сообщение об окончании отписки ListenerMessage_1 от глобального обьекта SendingMessage_1.";
            public const string x10003 = "Вы начинаете подписывать ListenerMessage_1 на глобальный обьект SendingMessage. Ожидаете что текущее состояние " +
                "ListenerMessage_1 SubscribeManager будет {0}, но оно {1}.";
            public const string x10004 = "Вы ожидаете пока глобальный обьект SendingMessage_1 отчитается о том что он подписал ваш ListenerMessage_1 находясь в состоянии" +
                "{0}, но текущее состояние {1}. Скорее всего пришло лишнее продублированое сообщение.";
            public const string x10005 = "Когда вы отправляете регистрационое/ые сообщения на отписку, ваше состояние должно быть {0}, но в данный момент оно {1}.";
            public const string x10006 = "Вы можете начать получение сообщений из Sending_Message_1 об оконачании отписки Listener_Message_1 когда находитесь в состоянии {0}, но в данный момент вынаходитесь в состоянии {1}";
            public const string x10007 = "Вам пришло лишнее сообщение {0}.";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct HeaderManager
        {
            public const string x10001 = "{0} обьект не может быть Independent так как является частью данного обьекта " +
                "и полностью связан с его жизненыеми процессами.";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct SubscribeToPoll
        {
            public const string x10001 = "Вы уже отписались от пула который обеспечивал работу {0}.";
            public const string x10002 = "Неизвестный тип информирования {0}.";
            public const string x10003 = "Вы можете создовать билеты только находитесь в состянии {0}, в данный момент вы находитесь в состоянии {1}.";
            public const string x10004 = "Создовать билеты для подписки на пуллы токлько когда обьект находится в состоянии {0} или {1}, а ниходится в {2}.";
            public const string x10005 = "Вызвать метод который зарегистрирует подписки в пулл можно только когда обьект находится в состоянии {0}, в данный момент он находится в состоянии {1}.";
            public const string x10006 = "Запустить регистрацию на подписки в пуллы можно только когда менеджер находится в состоянии {0}, а он находится в состоянии {1}.";
            public const string x10007 = "Вы зарегестировали на подписку {0} билетов, но получили лишний/лишние билеты.";
            public const string x10008 = "Вы можете получить сообщения об подписании в пулл когда обьект находится в состоянии {0}, но в данный момент он находится в состоянии {1}.";
            public const string x10009 = "Вы можете получать сообщения об подписании в пулл когда менержер находится в состоянии {0}, но в данный момент он находится в стостоянии {1}. ";
            public const string x10010 = "Вы можете получать сообщения об отписании из пулла, только когда обьект находятся в состоянии {0}, но в данный момент он находится в состоянии {1}.";
            public const string x10011 = "Вы можете получать сообщения об отписании из пула, только когда менеджер находится в состоянии {0}, но он находится в состянии {1}.";
            public const string x10012 = "При получении сообщений об отписке вы получили одно лишее сообщение.";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct Tegs
        {
            public const string x10001 = "Вы уже добавили данному обьекту тег {0}.";
            public const string x10002 = "Теги можно добовлять только в методе Contruction().";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct LifeCyrcleActivity
        {
            public const string x10001 = "Не удалось вызвать метод {0} для {1}.";
            public const string x10002 = "Попытка уничтожения неизвестого типа обьекта {0}.";
            public const string x10003 = "Вы можете запустить метод Start() в текущем классе только если текущий жиненый цикл обьекта {0}, но в данный момент он {1}.";
            public const string x10004 = "Вы пытаетесь в конце метода Stop() обратится к несущесвующему типу обьекта ({0}).";
            public const string x10005 = "Вы не можете вызывать метод destroy() в методе Construction(), данный метод предназначен для создания Breach обьектов и установления связей с глобальными обьектами.";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct PollsHandler
        {
            public const string x10001 = "";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct MainObjectSendingEcho
        {
            public const string x10001 = "Приватный обработчик {0} не реализует интерфейс {1}.";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }

        public struct IIII
        {
            public const string x10001 = "";
            public const string x10002 = "";
            public const string x10003 = "";
            public const string x10004 = "";
            public const string x10005 = "";
            public const string x10006 = "";
            public const string x10007 = "";
            public const string x10008 = "";
            public const string x10009 = "";
            public const string x10010 = "";
            public const string x10011 = "";
            public const string x10012 = "";
            public const string x10013 = "";
            public const string x10014 = "";
            public const string x10015 = "";
            public const string x10016 = "";
            public const string x10017 = "";
            public const string x10018 = "";
        }
    }
}
