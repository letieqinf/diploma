import { useEffect, useRef, useState } from "react";

import "./VerticalTabs.css";

function VerticalTabs(props: { titles: Array<any>, collection: string, children: React.ReactNode, className: string }) {
    const [active, setActive] = useState<number>(0);

    const changeActive = (event: React.SyntheticEvent) => {
        if (event.target instanceof Element) {
            const target = event.target;

            const tabNames = document.querySelectorAll(`.vertical-tab.--${ props.collection }`);
            const tabContents = document.querySelector(`.vertical-tabs-content.--${ props.collection }`)?.children;

            if (tabNames && tabContents) {
                const index = Array.prototype.slice.call(tabNames).indexOf(target);
                if (index === -1)
                    return;

                tabNames[active].classList.remove('active');
                tabContents[active].classList.remove('active');

                tabNames[index].classList.add('active');
                tabContents[index].classList.add('active');

                setActive(index);
            }
        }
    }

    const onLoad = useRef(false);

    useEffect(() => {
        if (onLoad.current) {
            let firstChild = document.querySelector(`.vertical-tabs-content.--${ props.collection }`)?.firstChild;
            if (firstChild instanceof Element) {
                firstChild.classList.add('active')
            }
        }

        onLoad.current = true;
    }, []);

    return (            
        <>
            <div className="vertical-tabs-container">
                <ul className="vertical-tabs">
                    {   
                        props.titles.map((title, index) => (
                            <li className={`vertical-tab --${props.collection}` + (index === 0 ? " active" : "")} key={index} onClick={changeActive}>{ title }</li>
                        )) 
                    }
                </ul>
            </div>
            <div className={ `vertical-tabs-content --${props.collection}` }>
                {
                    props.children
                }
            </div>
        </>
    );
}

export default VerticalTabs;