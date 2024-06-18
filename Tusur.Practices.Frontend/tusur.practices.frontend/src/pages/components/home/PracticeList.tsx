import IPracticeDate from "../../interfaces/IPracticeDate";
import PracticeDateCard from "./PracticeDateCard";

type Categories = {
    'Текущие практики': IPracticeDate[] | undefined,
    'Предстоящие практики': IPracticeDate[] | undefined,
    'Завершенные практики': IPracticeDate[] | undefined
};

interface IPracticeList {
    dates: IPracticeDate[],
    onCardClick? : (date: IPracticeDate) => void
}

function PracticeList(props: IPracticeList) {
    const { dates, onCardClick } = props;

    const getDateStatus = (date: IPracticeDate): any => {
        let currentDate = new Date();

        let startsAt = new Date(date.startsAt);
        let endsAt = new Date(date.endsAt);

        if (startsAt < currentDate) {
            if (endsAt < currentDate)
                return "Завершенные практики";
            return "Текущие практики";
        }
        return "Предстоящие практики";
    }

    return (
        (() => {
            let categorized: Categories = {
                'Текущие практики': undefined,
                'Предстоящие практики': undefined,
                'Завершенные практики':  undefined
            };
            
            dates.forEach((date) => {
                let status = getDateStatus(date) as keyof Categories;

                if (categorized[status])
                    categorized[status]?.push(date);
                else 
                    categorized[status] = [date];
            });

            return (
                <>
                    {
                        Object.keys(categorized).map((category, catIndex) => {
                            return (
                                categorized[category as keyof Categories] ?
                                <div className="content-block" key={`div-${catIndex}`}>
                                    <div className="block-title" key={`div-${catIndex}`}>
                                        <h2 key={`h2-${catIndex}`}>{ category }</h2>
                                    </div>
                                    <ul className="practice-list" key={`ul-${catIndex}`}>
                                        {
                                            categorized[category as keyof Categories]?.map((date, index) => {
                                                return (
                                                    <li key={index} className="practice-container">
                                                        <PracticeDateCard 
                                                            key={index} 
                                                            practice={date} 
                                                            onClick={ onCardClick ? () => onCardClick(date) : undefined }
                                                        />
                                                    </li>
                                                );
                                            })
                                        }
                                    </ul>
                                </div> : <></>
                            );
                        })
                    }
                </>
            );
        })()
    );
}

export default PracticeList;