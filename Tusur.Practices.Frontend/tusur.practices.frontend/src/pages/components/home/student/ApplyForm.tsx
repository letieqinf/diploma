import { useEffect, useState } from "react";
import IPracticeDate from "../../../interfaces/IPracticeDate";
import CustomCheckbox from "../../layout/CustomCheckbox";
import CustomInput from "../../layout/CustomInput";
import IOrganization from "../../../interfaces/IOrganization";

import "./ApplyForm.css";
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

function ApplyForm(props: { dates: IPracticeDate[] }) {
    const { dates } = props;
    const axiosPrivate = useAxiosPrivate();

    const [organizations, setOrganizations] = useState<IOrganization[]>();

    const [applyFormInputs, setApplyFormInputs] = useState<{ 
        practiceDateId: string | null,
        organizationId: string | null,
        organizationName: string,
        organizationAddress: string
    }>({
        practiceDateId: null,
        organizationId: null,
        organizationName: '',
        organizationAddress: ''
    });

    const [isOrgNotExist, setIsOrgNotExist] = useState<boolean>(false);
    const [isOrgNameFocused, setIsOrgNameFocused] = useState<boolean>(false);

    const handleSelectChange = (event: React.SyntheticEvent) => {
        let target = event.target;
        if (target instanceof HTMLSelectElement) {
            setApplyFormInputs({
                ...applyFormInputs,
                practiceDateId: target.value
            })
        }
    }
    
    const handleInputChange = (event: any) => {
        let target = event.target;
        setApplyFormInputs({
            ...applyFormInputs,
            [target.name]: [target.value]
        });
    }

    const handleOrganizationClick = (org: IOrganization) => {
        setApplyFormInputs({
            ...applyFormInputs,
            organizationId: org.id,
            organizationName: org.organizationName,
            organizationAddress: org.organizationAddress
        });
    }

    const handleSubmit = (event: React.SyntheticEvent) => {
        event.preventDefault();

        const target = event.target as HTMLButtonElement;

        const createApplication = async () => {
            try {
                await axiosPrivate.post('api/applications', {
                    organizationId: applyFormInputs.organizationId,
                    organizationName: applyFormInputs.organizationName[0],
                    organizationAddress: applyFormInputs.organizationAddress[0],
                    practiceDateId: applyFormInputs.practiceDateId,
                    isDraft: target.value === "save"
                });
                window.location.reload();
            } catch (error) {
                console.error(error);
            }
        }

        createApplication();
    }

    useEffect(() => {
        const getOrganizations = async () => {
            try {
                const response = await axiosPrivate.get('api/organizations?approvedOnly=true');
                setOrganizations(response.data);
            } catch (error) {
                console.error(error)
            }
        }

        if (isOrgNameFocused) {
            getOrganizations();
        }
    }, [isOrgNameFocused]);

    return (
        <form className="apply-form-container">
            <div className="practice-select-container apply-form-part">
                <div className="title title-frame blue">
                    <h3>Практика</h3>
                </div>
                <div className="practice">
                    <select onChange={handleSelectChange} name="practice-select" id="practice-select" defaultValue={""}>
                        <option value="" disabled>Выберите практику</option>
                        {
                            dates.map((date, index) => {
                                if (new Date(date.endsAt) >= new Date()) {
                                    return <option key={index} value={ date.id }>{ `${date.kind} ${date.type}` }</option>
                                }
                            })
                        }
                    </select>
                </div>
            </div>
            <div className="organization-container apply-form-part">
                <div className="title title-frame blue">
                    <h3>Профильная организация</h3>
                </div>
                <div className="organization">
                    <div className="organization-exist-checkbox">
                        <CustomCheckbox label="Практики нет в базе данных" onClick={() => setIsOrgNotExist(() => !isOrgNotExist)} checked={isOrgNotExist} />
                    </div>
                    <div className="organization-inputs">
                        <CustomInput 
                            label="Название"
                            name="organizationName"
                            collection="apply-form-organization"
                            value={applyFormInputs.organizationName}
                            className="item"
                            onChange={handleInputChange}
                            onFocus={() => setIsOrgNameFocused(true)}
                            onBlur={() => setIsOrgNameFocused(false)}
                        />
                        {
                            isOrgNotExist
                            ? 
                                <CustomInput 
                                    label="Адрес"
                                    name="organizationAddress"
                                    collection="apply-form-organization"
                                    value={applyFormInputs.organizationAddress}
                                    className="item"
                                    onChange={handleInputChange}
                                />
                            : isOrgNameFocused && organizations
                                ?
                                    <div className="organization-selector-container">
                                        {
                                            (() => {
                                                let orgs = organizations.map((org, index) => {
                                                    if (org.organizationName.includes(
                                                        applyFormInputs.organizationName[0]
                                                    )) {
                                                        return (
                                                            <p 
                                                                key={index}
                                                                onMouseDown={() => handleOrganizationClick(org)}
                                                            >
                                                                { org.organizationName }
                                                            </p>
                                                        );
                                                    }
                                                })

                                                if (orgs.length <= 20) {
                                                    return orgs;
                                                }
                                            })()
                                        }
                                    </div>
                                :
                                    <></>
                        }
                    </div>
                </div>
            </div>
            <div className="buttons-container apply-form-part">
                    <button type="button" value="create" onClick={handleSubmit} className="button">Отправить</button>
                    <button type="button" value="save" onClick={handleSubmit} className="button white">Сохранить</button>
                </div>
        </form>
    );
}

export default ApplyForm;