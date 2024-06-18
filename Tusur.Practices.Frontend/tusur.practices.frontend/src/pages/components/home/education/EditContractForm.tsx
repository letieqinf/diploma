import { useEffect, useState } from "react";
import IPracticeProfile from "../../../interfaces/IPracticeProfile";
import "./EditContractForm.css";
import { Organizer, OrganizerItem } from "../../layout/organizer/Organizer";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { CustomTextBox } from "../../layout/CustomTextBox";

interface IEditContractForm {
    profile: IPracticeProfile
}

type ContentInputs = {
    uniResp: string[],
    orgResp: string[],
    uniCan: string[],
    orgCan: string[]
}

export const EditContractForm: React.FunctionComponent<IEditContractForm> = ( props ) => {
    const { profile } = props;

    const [content, setContent] = useState<IContent>({
        id: ``,
        uniResp: 
            `Не позднее, чем за 10 рабочих дней до начала практики представить в Профильную организацию поименные списки обучающихся, направляемых для прохождения практики, программу практики, график прохождения практик.
            Назначить руководителя по практической подготовке от Университета, который:
            – обеспечивает организацию практики обучающихся;
            – контролирует участие обучающихся в выполнении определенных видов работ, связанных с будущей профессиональной деятельностью;
            – оказывает методическую помощь обучающимся при выполнении определенных видов работ, связанных с будущей профессиональной деятельностью;
            – несет ответственность совместно с руководителем практики от профильной организации за реализацию программы практики, за жизнь и здоровье обучающихся и работников Университета, соблюдение ими правил противопожарной безопасности, правил охраны труда, техники безопасности и санитарно-эпидемиологических правил и гигиенических нормативов.
            При смене руководителя практики в 7-дневный срок сообщить об этом Профильной организации.
            Направить обучающихся в Профильную организацию для освоения программы практики.
            Расследовать и учитывать несчастные случаи, если они произойдут со обучающимися в период прохождения практической подготовки на территории профильной организации.`,
        orgResp: `При смене лица руководителя практики от профильной организации в 7-дневный срок сообщить об этом Университету.
            Обеспечить реализацию программы практики со стороны профильной организации;
            Обеспечить безопасные условия реализации программы практики, выполнение правил противопожарной безопасности, правил охраны труда, техники безопасности и санитарно-эпидемиологических правил и гигиенических нормативов;
            Проводить оценку условий труда на рабочих местах, используемых при реализации программы практики;
            Ознакомить обучающихся с правилами внутреннего трудового распорядка профильной организации и иными необходимыми локальными нормативными актами;
            Провести инструктаж обучающихся по охране труда и технике безопасности.
            Предоставить обучающимся и руководителю практики от Университета возможность пользоваться помещениями Профильной организации, согласованными Сторонами (приложение 2 к настоящему Договору), а также находящимися в них оборудованием и техническими средствами обучения.
            Обо всех случаях нарушения обучающимися правил внутреннего трудового распорядка, охраны труда и техники безопасности сообщить руководителю практики от университета;
            Расследовать и учитывать несчастные случаи, если они произойдут с обучающимися в период прохождения практики в Профильной организации в соответствии с Положением о расследовании и учёте несчастных случаев на производстве.
            Не допускать использования обучающихся на должностях, не предусмотренных программой практики и не имеющих отношения к направлению подготовки/специальности обучающихся.`,
        uniCan: ``,
        orgCan: ``
    });

    const [contentInputs, setContentInputs] = useState<ContentInputs>();
    const [currentPage, setCurrentPage] = useState<number>(0);

    useEffect(() => {
        if (content) {
            setContentInputs({
                uniResp: content.uniResp.match(/(?:(?:[\p{L}]+)(?:\p{P}*[^\n])*(?:\n\s*[-–—]\s*)*)+/ug) as string[],
                orgResp: content.orgResp.match(/(?:(?:[\p{L}]+)(?:\p{P}*[^\n])*(?:\n\s*[-–—]\s*)*)+/ug) as string[],
                uniCan: content.uniCan.match(/(?:(?:[\p{L}]+)(?:\p{P}*[^\n])*(?:\n\s*[-–—]\s*)*)+/ug) as string[],
                orgCan: content.orgCan.match(/(?:(?:[\p{L}]+)(?:\p{P}*[^\n])*(?:\n\s*[-–—]\s*)*)+/ug) as string[],
            });
        }
    }, []);
 
    return (
        <>
            <Organizer currentValue={ currentPage } onChange={ () => setCurrentPage(0) }>
                <OrganizerItem value={ 0 }>
                    <div className={ `contract-editor-container` }>
                        <ul className={ `contract-editor-categories` }>
                            <li className={ `category` } onClick={ () => setCurrentPage(1) }>
                                <div className={ `category-header` }>
                                    Обязанности университета
                                </div>
                                <div className={ `category-link` }>
                                    <FontAwesomeIcon icon={ faChevronRight } />
                                </div>
                            </li>
                            <li className={ `category` } onClick={ () => setCurrentPage(2) }>
                                <div className={ `category-header` }>
                                    Обязанности организации
                                </div>
                                <div className={ `category-link` }>
                                    <FontAwesomeIcon icon={ faChevronRight } />
                                </div>
                            </li>
                            <li className={ `category` } onClick={ () => setCurrentPage(3) }>
                                <div className={ `category-header` }>
                                    Права университета
                                </div>
                                <div className={ `category-link` }>
                                    <FontAwesomeIcon icon={ faChevronRight } />
                                </div>
                            </li>
                            <li className={ `category` } onClick={ () => setCurrentPage(4) }>
                                <div className={ `category-header` }>
                                    Права организации
                                </div>
                                <div className={ `category-link` }>
                                    <FontAwesomeIcon icon={ faChevronRight } />
                                </div>
                            </li>
                        </ul>
                        <div>
                            <button type={ `button` } className={ `button` }>Сохранить</button>
                        </div>
                    </div>
                </OrganizerItem>
                <OrganizerItem value={ 1 } parentValue={ 0 } title={ `Обязанности университета` }>
                    <div className={ `contract-fields-container` }>
                        {
                            contentInputs?.uniResp.map((entity, index) => {
                                return (
                                    <div className={ `contract-field-controller` } key={ index }>
                                        <CustomTextBox content={ entity } className={ `contract-field-textbox` } />
                                        <button  
                                            type={ `button` }
                                            className={ `button green` }
                                            onClick={ () => setContentInputs({
                                                ...contentInputs,
                                                uniResp: contentInputs.uniResp.filter(value => value !== entity)
                                            }) }
                                        >
                                        Удалить
                                        </button>
                                    </div>
                                );
                            })
                        }
                        <div>
                            <button type={ `button` } className={ `button` }>Добавить</button>
                        </div>
                    </div>
                </OrganizerItem>
                <OrganizerItem value={ 2 } parentValue={ 0 } title={ `Обязанности организации` }>
                    3
                </OrganizerItem>
                <OrganizerItem value={ 3 } parentValue={ 0 } title={ `Права университета` }>
                    4
                </OrganizerItem>
                <OrganizerItem value={ 4 } parentValue={ 0 } title={ `Права организации` }>
                    5
                </OrganizerItem>
            </Organizer>
        </>
    );
}